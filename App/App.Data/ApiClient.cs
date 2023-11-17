
using System.Text.Json;
using System.Text.RegularExpressions;

namespace App.Data
{
    public interface IApiClient
    {
        public Task<T?> GetJsonAsync<T>(string uri);
    }

    public class Apiclient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public Apiclient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetJsonAsync<T>(string uri)
        {
            var jsonString = await _httpClient.GetStringAsync(uri);

            var jsonStringCleaned = CleanJson(jsonString);
            return JsonSerializer.Deserialize<T>(jsonStringCleaned);
        }

        // Try and clean the JSON string by fixing any issues with keys not being surrounded with quotes
        private static string CleanJson(string json) => Regex.Replace(json, @"\b(\w+)(:)", "\"$1\"$2");
    }
}
