using System.Net;

namespace PixivApi.Tests;

internal static class DownloadTestHelpers
{
    /// <summary>
    /// Configures the handler so that the first <paramref name="failingAttempts"/> requests fail
    /// partway through the body and all subsequent requests return a complete payload.
    /// </summary>
    public static void ConfigureFlakyDownload(
        this TestHttpMessageHandler handler,
        string url,
        byte[] partialAttempt,
        int failAfterBytes,
        byte[] successfulAttempt,
        int failingAttempts = 1)
    {
        var attempts = 0;
        handler.When(url, () =>
        {
            if (attempts++ < failingAttempts)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new FailingReadStream(partialAttempt, failAfterBytes))
                };
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(successfulAttempt)
            };
        });
    }

    public static byte[] CreatePayload(byte value, int length) => Enumerable.Repeat(value, length).ToArray();
}

internal sealed class FailingReadStream : Stream
{
    private readonly byte[] _data;
    private readonly int _failAfterBytes;
    private int _position;

    public FailingReadStream(byte[] data, int failAfterBytes)
    {
        _data = data;
        _failAfterBytes = failAfterBytes;
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_position >= _failAfterBytes)
        {
            throw new IOException("Simulated connection failure.");
        }

        var bytesToRead = Math.Min(count, _failAfterBytes - _position);
        Array.Copy(_data, _position, buffer, offset, bytesToRead);
        _position += bytesToRead;
        return bytesToRead;
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (_position >= _failAfterBytes)
        {
            throw new IOException("Simulated connection failure.");
        }

        var bytesToRead = Math.Min(buffer.Length, _failAfterBytes - _position);
        _data.AsSpan(_position, bytesToRead).CopyTo(buffer.Span);
        _position += bytesToRead;
        return ValueTask.FromResult(bytesToRead);
    }

    public override void Flush()
    {
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

internal sealed class NonSeekableWriteStream : Stream
{
    private readonly Stream _inner;

    public NonSeekableWriteStream(Stream inner)
    {
        _inner = inner;
    }

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush() => _inner.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);

    public override void Write(ReadOnlySpan<byte> buffer) => _inner.Write(buffer);

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        => _inner.WriteAsync(buffer, cancellationToken);
}
