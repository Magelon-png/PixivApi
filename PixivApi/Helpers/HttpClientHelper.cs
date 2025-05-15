using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Scighost.PixivApi.Helpers;

/// <summary>
/// 
/// </summary>
internal static class HttpClientHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal static ResiliencePipeline GetResiliencePipeline()
    {
        var builder = new ResiliencePipelineBuilder();
        builder.AddRetry(options: new RetryStrategyOptions()
        {
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
            Delay = TimeSpan.FromMilliseconds(100)
        });

        builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions()
        {
            FailureRatio = 0.5,
            MinimumThroughput = 10,
            SamplingDuration = TimeSpan.FromSeconds(10),
            BreakDuration = TimeSpan.FromSeconds(30)
        });
        
        return builder.Build();

    }
}