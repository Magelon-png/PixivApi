using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Text.Json;
using Scighost.PixivApi;

public class CurlImpersonateHandler : HttpMessageHandler
{
    private readonly string _curlPath;
    private readonly string _impersonationProfile; 

    public CurlImpersonateHandler(string? curlPath = null, string impersonationProfile = "chrome")
    {
        _impersonationProfile = impersonationProfile;
        _curlPath = ResolveCurlImpersonatePath(curlPath, impersonationProfile);
    }

    private static string ResolveCurlImpersonatePath(string? manualPath, string profile)
    {
        if (!string.IsNullOrWhiteSpace(manualPath) && File.Exists(manualPath))
            return manualPath;
        
        try
        {
            string found = FindCurlImpersonateExecutable(profile);
            if (!string.IsNullOrEmpty(found))
                return found;
        }
        catch {  }
        
        string os, arch, ext = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) os = "windows";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) os = "linux";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) os = "macos";
        else throw new PlatformNotSupportedException("Système d'exploitation non supporté.");

        switch (RuntimeInformation.OSArchitecture)
        {
            case Architecture.X64:
                arch = "x64";
                break;
            case Architecture.Arm64:
                arch = "arm64";
                break;
            default:
                throw new PlatformNotSupportedException("Architecture CPU non supportée.");
        }

        if (os == "windows")
            ext = ".exe";
        
        string appBaseDir = AppContext.BaseDirectory;

        string bundledPath = Path.Combine(appBaseDir, "binaries", os, arch, $"curl-impersonate-{profile}{ext}");

        if (File.Exists(bundledPath))
            return bundledPath;

        throw new FileNotFoundException(
            $"Le binaire curl-impersonate (profile: {profile}) est introuvable.\n" +
            $"- Chemin fourni : \"{manualPath}\" \n" +
            $"- PATH système et emplacements connus\n" +
            $"- Bundle attendu : \"{bundledPath}\"");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var argsList = new System.Collections.Generic.List<string>
        {
            //$"--impersonate-{_impersonationProfile}",
            "-i", "-sSL",
            "-X", request.Method.Method
        };

        argsList.Add(ShellEscape(request.RequestUri!.ToString()
            .Replace(" ", "%20")));

        foreach (var header in request.Headers)
            foreach (var value in header.Value)
            {
                argsList.Add("-H");
                argsList.Add(ShellEscape($"{header.Key}: {value}"));
            }
        if (request.Content != null)
        {
            foreach (var header in request.Content.Headers)
                foreach (var value in header.Value)
                {
                    argsList.Add("-H");
                    argsList.Add(ShellEscape($"{header.Key}: {value}"));
                }
        }

        bool hasBody = request.Content != null;

        if (hasBody)
        {
            argsList.Add("--data-binary");
            argsList.Add("@-");
        }

        var psi = new ProcessStartInfo
        {
            FileName = _curlPath,
            RedirectStandardInput = hasBody,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(_curlPath)

        };


        psi.ArgumentList.Clear();
        foreach (string arg in argsList)
            psi.ArgumentList.Add(arg);


        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                var chmod = new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{_curlPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(chmod))
                    await proc.WaitForExitAsync(cancellationToken);
            }
            catch { }
        }

        var process = new Process { StartInfo = psi };
        process.Start();

        // POST and PUT
        if (hasBody)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var stdin = process.StandardInput.BaseStream;
                    using var contentStream = await request.Content!.ReadAsStreamAsync(cancellationToken);
                    await contentStream.CopyToAsync(stdin, 81920, cancellationToken);
                    await stdin.FlushAsync(cancellationToken);
                }
                catch
                {
                    try { process.StandardInput.Close(); } catch { }
                }
            }, cancellationToken);
        }

        var stdout = process.StandardOutput.BaseStream;

        var response = new HttpResponseMessage();
        var headerBytes = new List<byte>();
        byte[] buffer = new byte[1];
        bool headersDone = false;
        int consecutiveNewlines = 0;

        while (!headersDone)
        {
            int bytesRead = await stdout.ReadAsync(buffer, 0, 1, cancellationToken);
            if (bytesRead == 0)
                break;

            byte b = buffer[0];
            headerBytes.Add(b);

            if (b == '\n')
            {
                consecutiveNewlines++;
                if (consecutiveNewlines >= 2)
                {
                    headersDone = true;
                }
            }
            else if (b == '\r')
            {
            }
            else
            {
                consecutiveNewlines = 0;
            }
        }

        //Header parsing
        string headersText = Encoding.UTF8.GetString(headerBytes.ToArray());
        using (var sr = new StringReader(headersText))
        {
            bool first = true;
            string? headerLine;
            while ((headerLine = sr.ReadLine()) != null)
            {
                if (first)
                {
                    var status = headerLine.Split(' ', 3);
                    if (status.Length >= 2 && int.TryParse(status[1], out int statusCode))
                        response.StatusCode = (System.Net.HttpStatusCode)statusCode;
                    first = false;
                    continue;
                }
                if (string.IsNullOrEmpty(headerLine))
                    break;
                int idx = headerLine.IndexOf(':');
                if (idx > 0 && idx + 1 < headerLine.Length)
                {
                    string name = headerLine.Substring(0, idx).Trim();
                    string value = headerLine.Substring(idx + 1).Trim();
                    if (!response.Headers.TryAddWithoutValidation(name, value))
                        response.Content?.Headers.TryAddWithoutValidation(name, value);
                }
            }
        }

        var contentStream = new ProcessStream(stdout, process);
        response.Content = new StreamContent(contentStream);
        
        return response;
    }
    
    public static string FindCurlImpersonateExecutable(string profile)
    {
        string exeName =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"curl-impersonate-{profile}.exe" : $"curl-impersonate-{profile}";
        var paths = (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator);
        foreach (var dir in paths)
        {
            string fullPath = Path.Combine(dir, exeName);
            if (File.Exists(fullPath))
                return fullPath;
        }
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var candidates = new[]
        {
            Path.Combine(home, exeName),
            "/usr/local/bin/" + exeName,
            "/usr/bin/" + exeName,
            "/opt/homebrew/bin/" + exeName
        };
        foreach (var cap in candidates)
            if (File.Exists(cap))
                return cap;
        throw new FileNotFoundException($"Impossible de trouver '{exeName}' dans le PATH ni dans les emplacements habituels.");
    }

    public static string ShellEscape(string input)
    {
        if (string.IsNullOrEmpty(input)) return "\"\"";
    
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return input;
        }
        else
        {
            if (input.Any(ch => char.IsWhiteSpace(ch) || "'\"\\$!`".Contains(ch)))
            {
                return "'" + input.Replace("'", "'\\''") + "'";
            }
            else
            {
                return input;
            }
        }
    }

    private class ProcessStream : Stream
    {
        private readonly Stream _stdout;
        private readonly Process _proc;
        private readonly StringBuilder _errorBuffer;
        private readonly Task _errorTask;
        private bool _disposed = false;
        private bool _processExited = false;

        public ProcessStream(Stream stdout, Process proc)
        {
            _stdout = stdout;
            _proc = proc;
            _errorBuffer = new StringBuilder();
            
            _errorTask = Task.Run(async () =>
            {
                try
                {
                    using var errorReader = proc.StandardError;
                    char[] buffer = new char[1024];
                    int charsRead;
                    while ((charsRead = await errorReader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        _errorBuffer.Append(buffer, 0, charsRead);
                    }
                }
                catch {  }
            });
        }
        
        public override bool CanRead => _stdout.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => _stdout.Flush();
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckProcessStatus();
            return _stdout.Read(buffer, offset, count);
        }
        
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            CheckProcessStatus();
            return await _stdout.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
        }
        
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        public override int Read(Span<byte> buffer)
        {
            CheckProcessStatus();
            return _stdout.Read(buffer);
        }
        
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            CheckProcessStatus();
            return await _stdout.ReadAsync(buffer, cancellationToken);
        }
#endif
        
        private void CheckProcessStatus()
        {
            if (_processExited) return;
            
            if (_proc.HasExited)
            {
                _processExited = true;
                
                try
                {
                    _errorTask.Wait(TimeSpan.FromSeconds(1));
                }
                catch { }
                
                if (_proc.ExitCode != 0)
                {
                    string errorMessage = _errorBuffer.ToString();
                    var exception = CreateExceptionFromCurlError(_proc.ExitCode, errorMessage);
                    throw exception;
                }
            }
        }
        
        private static Exception CreateExceptionFromCurlError(int exitCode, string errorMessage)
        {
            return exitCode switch
            {
                1 => new InvalidOperationException($"Curl: Protocole non supporté ou erreur d'initialisation. {errorMessage}"),
                2 => new InvalidOperationException($"Curl: Échec d'initialisation. {errorMessage}"),
                3 => new ArgumentException($"Curl: URL malformée. {errorMessage}"),
                5 => new InvalidOperationException($"Curl: Impossible de résoudre le proxy. {errorMessage}"),
                6 => new InvalidOperationException($"Curl: Impossible de résoudre l'hôte. {errorMessage}"),
                7 => new HttpRequestException($"Curl: Impossible de se connecter à l'hôte. {errorMessage}"),
                22 => new HttpRequestException($"Curl: Erreur HTTP retournée par le serveur. {errorMessage}"),
                28 => new TimeoutException($"Curl: Timeout d'opération. {errorMessage}"),
                35 => new InvalidOperationException($"Curl: Erreur SSL/TLS lors de la connexion. {errorMessage}"),
                51 => new UnauthorizedAccessException($"Curl: Certificat peer invalide ou autre erreur SSL. {errorMessage}"),
                52 => new InvalidOperationException($"Curl: Le serveur n'a rien retourné. {errorMessage}"),
                53 => new InvalidOperationException($"Curl: Erreur crypto SSL. {errorMessage}"),
                54 => new InvalidOperationException($"Curl: Erreur de vérification SSL. {errorMessage}"),
                55 => new IOException($"Curl: Erreur d'envoi réseau. {errorMessage}"),
                56 => new IOException($"Curl: Erreur de réception réseau. {errorMessage}"),
                58 => new InvalidOperationException($"Curl: Problème avec le certificat local. {errorMessage}"),
                59 => new InvalidOperationException($"Curl: Impossible d'utiliser le chiffrement SSL. {errorMessage}"),
                60 => new UnauthorizedAccessException($"Curl: Certificat peer invalide. {errorMessage}"),
                77 => new InvalidOperationException($"Curl: Erreur CA cert (problème de lecture du fichier). {errorMessage}"),
                78 => new HttpRequestException($"Curl: URL retournée non trouvée. {errorMessage}"),
                _ => new InvalidOperationException($"Curl: Erreur inconnue (code {exitCode}). {errorMessage}")
            };
        }
        
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        CheckProcessStatus();
                    }
                    catch { }
                    
                    _stdout.Dispose();
                    
                    try
                    {
                        if (!_proc.HasExited)
                        {
                            _proc.WaitForExit(3000);
                        }
                        _errorTask.Wait(TimeSpan.FromSeconds(1));
                    }
                    catch { }
                    finally
                    {
                        _proc.Dispose();
                    }
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}