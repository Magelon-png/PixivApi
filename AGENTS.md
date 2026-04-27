# PixivApi Agent Guide

## Architecture Overview

**PixivApi** is a C# wrapper for Pixiv's web API with multi-target support (.NET 8, 9, 10). The core design revolves around:

- **Single Entry Point**: `PixivClient` in `Clients/PixivClient.cs` is the main request dispatcher for all API interactions
- **Response Wrapper Pattern**: All API responses wrapped in `PixivResponseWrapper<T>` (error handling at envelope level, not HTTP status codes)
- **Domain Models**: Organized by feature - `Illust/`, `Novel/`, `User/`, `Search/`, `Fanbox/`, plus `Common/` for shared types
- **JSON Source Generation**: Uses `[JsonSourceGenerationOptions]` via `SerializerContexts/` with strict AOT compatibility

## Critical Development Workflows

### Building & Testing
```bash
# Run all tests (unit + integration)
dotnet test

Note: Never run the integration tests automatically. Never ask if the user wants to run them. It should be a manual operation from the user.

# Build solution
dotnet build

# Pack for NuGet (main branch only via Azure Pipelines)
dotnet pack
```

### Authentication Flow
Pixiv uses Cloudflare protection; cookies must be obtained via browser login. Valid cookies require three keys:
```csharp
// Required keys in cookie: __cf_bm, cf_clearance, PHPSESSID
PixivClient.ValidateCookie(cookieString); // Pre-validates

// For write operations (follow, bookmark):
await client.GetTokenAsync(); // Must call before POST/PUT operations
```

### Test Infrastructure
- **Unit Tests**: `PixivApi.Tests/` uses MSTest with mock payloads in `Payloads/` directory
- **Integration Tests**: `PixivApi.IntegrationTests/` for live API calls (requires real credentials). Should never be ran by an agent.
- **Test Handler**: `TestHttpMessageHandler` in tests allows injecting mock responses without real HTTP calls

## Project-Specific Patterns

### Response Error Handling
Unlike typical REST APIs, Pixiv returns HTTP 200 with error wrapped in JSON:
```csharp
// CommonGetAsync unwraps and throws PixivException on wrapper.Error == true
private async Task<T> CommonGetAsync<T>(string url, JsonTypeInfo<...> jsonTypeInfo)
{
    var wrapper = await _resiliencePipeline.ExecuteAsync(...);
    if (wrapper?.Error ?? true)
        throw new PixivException(wrapper?.Message);
    return wrapper.Body;
}
```

### Resilience & Retry Logic
Uses `Microsoft.Extensions.Http.Resilience` with `ResiliencePipeline` (configured in `HttpClientHelper.cs`). All API calls wrapped with retry policies.

### Image Download Handling
Two separate `HttpClient` instances:
- `_httpClient`: Authenticated API requests with cookie headers
- `_downloadHttpClient`: Image downloads requiring `Referer: https://www.pixiv.net/` hotlink protection header

### Asynchronous Streaming for Large Datasets
Some endpoints (recommendations) return paginated data via async streams:
```csharp
await foreach (IEnumerable<IllustProfile> illusts in client.(...))
{
    // Process batch without loading all at once
}
```

### Client Variants
- **PixivClient**: Main v1 API with authentication
- **PixivClientV2**: API endpoints used by the mobile app (V2 models in `Models/V2/`)
- **FanboxClient**: Separate Fanbox service with own serializer context

### Binary Dependency Management
curl-impersonate binaries packaged per-platform in `binaries/` (linux/macos/windows × arm64/x64). Handled by `CurlImpersonateHandler` for Cloudflare bypass during login.

## Key Files & Boundaries

| File/Folder | Purpose |
|---|---|
| `Clients/PixivClient.cs` | ~1349 lines, main dispatcher; method groups: User, Illust, Novel, Search, Bookmark |
| `Models/` | Data contracts organized by domain; records use `[JsonPropertyName]` + `[JsonNumberHandling]` for deserialization |
| `SerializerContexts/` | JSON serializers via `JsonSerializerContext`; must register all response types |
| `Exceptions/PixivException.cs` | Thrown when API response has `error: true` regardless of HTTP status |
| `Helpers/HttpClientHelper.cs` | Resilience pipeline, retry policies |

## Type Safety & Serialization

- **Records**: Immutable, used throughout for API DTOs
- **Global Using**: `System.Text.Json.Serialization` in `GlobalUsing.cs` for attribute access
- **Number Handling**: Properties like IDs use `[JsonNumberHandling(...WriteAsString)]` for compatibility with string IDs from API
- **Null Safety**: Project configured with `<Nullable>enable</Nullable>`

## Integration Points

- **Cloudflare**: Authentication requires valid cookies; `CurlImpersonateHandler` used for automation
- **External URLs**: Image URLs contain domain-specific handling (hotlink protection)
- **Async Streams**: Some endpoints use `IAsyncEnumerable<T>` for pagination
- **NuGet Publishing**: Automated via Azure Pipelines to Nuget.org, GitHub Packages, and private ADO feed (main branch only)

## Versioning & Compatibility

- Project targets net8.0, net9.0, net10.0
- V2 breaking changes planned ( namespace reorganization)
- Cross-platform binary support (platform-specific curl-impersonate binaries in package)

