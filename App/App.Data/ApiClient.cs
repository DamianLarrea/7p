
using Polly.Registry;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace App.Data
{
    public interface IApiClient
    {
        public Task<T?> GetJsonAsync<T>(string uri, CancellationToken token);
    }

    public class Apiclient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

        public Apiclient(HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipelineProvider)
        {
            _httpClient = httpClient;
            _resiliencePipelineProvider = resiliencePipelineProvider;
        }

        public async Task<T?> GetJsonAsync<T>(string uri, CancellationToken token)
        {
            var pipeline = _resiliencePipelineProvider.GetPipeline("tech-test-pipeline");

            return await pipeline.ExecuteAsync(
                async ct => await FetchAsync<T>(uri, ct),
                token
            );
        }

        private async Task<T?> FetchAsync<T>(string uri, CancellationToken token)
        {
            var jsonString = await _httpClient.GetStringAsync(uri, token);

            var jsonStringCleaned = CleanJson(jsonString);

            return JsonSerializer.Deserialize<T>(jsonStringCleaned);
        }

        // Try and clean the JSON string by fixing any issues with keys not being surrounded with quotes
        private static string CleanJson(string json) => Regex.Replace(json, @"\b(\w+)(:)", "\"$1\"$2");
    }
}
