
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;

namespace App.Console.Extensions
{
    internal static class PollyRegistration
    {
        public static HostApplicationBuilder AddPollyPipeline(this HostApplicationBuilder builder, string pipelineName)
        {
            var opts = builder.Configuration.GetSection(nameof(PollyOptions)).Get<PollyOptions>() ?? new PollyOptions();

            builder.Services.AddResiliencePipeline(pipelineName, pipelineBuilder =>
            {
                pipelineBuilder
                    .AddRetry(new RetryStrategyOptions { MaxRetryAttempts = opts.RetryCount })
                    .AddTimeout(TimeSpan.FromSeconds(opts.DelayBetweenAttemptsInSeconds));
            });

            return builder;
        }

        private class PollyOptions
        {
            public int DelayBetweenAttemptsInSeconds { get; set; } = 30;

            public int RetryCount { get; set; } = 1;
        }
    }
}
