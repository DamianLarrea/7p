using App.Core;
using App.Data.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace App.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly UserApiOptions _apiOptions;
        private readonly CacheOptions _cacheOptions;

        public UserRepository(
            IHttpClientFactory httpClientFactory,
            IOptions<UserApiOptions> apiOptions,
            IOptions<CacheOptions> cacheOptions,
            IMemoryCache memoryCache
        )
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _apiOptions = apiOptions.Value;
            _cacheOptions = cacheOptions.Value;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = FetchUsersFromCache();

            if (users is not null) return users;

            var client = _httpClientFactory.CreateClient();

            var jsonString = await client.GetStringAsync(_apiOptions.Url);

            var jsonStringCleaned = CleanJson(jsonString);
            var dtos = JsonSerializer.Deserialize<IEnumerable<UserDto>>(jsonStringCleaned);

            users = dtos?.Select(MapToUser).ToList() ?? new List<User>();

            CacheUsers(users);

            return users;
        }

        private static User MapToUser(UserDto dto) => new User(dto.Id, dto.Age, dto.FirstName, dto.LastName, dto.Gender);

        // Try and clean the JSON string by fixing any issues with keys not being surrounded with quotes
        private static string CleanJson(string json) => Regex.Replace(json, @"\b(\w+)(:)", "\"$1\"$2");

        private void CacheUsers(IEnumerable<User> users)
            => _memoryCache.Set("users", users, DateTime.UtcNow.AddSeconds(_cacheOptions.TimeoutInSeconds));

        private IEnumerable<User>? FetchUsersFromCache() {
            _memoryCache.TryGetValue<IEnumerable<User>>("users", out var users); return users;
        }
    }
}