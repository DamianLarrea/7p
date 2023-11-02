using App.Core;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace App.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserApiOptions _apiOptions;

        public UserRepository(IHttpClientFactory httpClientFactory, IOptions<UserApiOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _apiOptions = options.Value;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<IEnumerable<UserDto>>(_apiOptions.Url);

            return response?.Select(MapToUser).ToList() ?? new List<User>();
        }

        private static User MapToUser(UserDto dto) => new User(dto.Id, dto.Age, dto.FirstName, dto.LastName, dto.Gender);
    }
}