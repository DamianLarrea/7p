using App.Core;
using App.Data.Caching;

namespace App.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IApiClient _apiClient;
        private readonly ICache _cache;

        public UserRepository(IApiClient apiClient, ICache cache)
        {
            _apiClient = apiClient;
            _cache = cache;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken token)
        {
            var users = FetchUsersFromCache();

            if (users is not null) return users;

            var dtos = await _apiClient.GetJsonAsync<IEnumerable<UserDto>>(string.Empty, token);

            users = dtos?.Select(MapToUser).ToList() ?? new List<User>();

            if (users.Any()) CacheUsers(users);

            return users;
        }

        private static User MapToUser(UserDto dto) => new User(dto.Id, dto.Age, dto.FirstName, dto.LastName, dto.Gender);

        private void CacheUsers(IEnumerable<User> users) => _cache.Set("users", users);

        private IEnumerable<User>? FetchUsersFromCache()
        {
            _cache.TryGetValue<IEnumerable<User>>("users", out var users); return users;
        }
    }
}