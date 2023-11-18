using App.Core;
using App.Data.Caching;
using FluentAssertions;
using Moq;

namespace App.Data.UnitTests
{
    public class UserRepositoryTests
    {
        private Mock<IApiClient> _apiClientMock;
        private Mock<ICache> _cacheMock;

        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _apiClientMock = new Mock<IApiClient>();
            _cacheMock = new Mock<ICache>();

            _userRepository = new UserRepository(_apiClientMock.Object, _cacheMock.Object);
        }

        [Test]
        public async Task GetUsers_GivenApiClientReturnsNull_ReturnsEmptyList()
        {
            _apiClientMock
                .Setup(client => client.GetJsonAsync<IEnumerable<UserDto>>(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<UserDto>?)null);

            var users = await _userRepository.GetUsers();

            // No need to check that users is not null as BeEmpty performs the check internally
            users.Should().BeEmpty();
        }

        [Test]
        public async Task GetUsers_GivenUserDtos_CorrectlyMapsToUsers()
        {
            var userDto = new UserDto { Age = 18, FirstName = "Test", Gender = "M", Id = 1, LastName = "User" };
            _apiClientMock
                .Setup(client => client.GetJsonAsync<IEnumerable<UserDto>>(It.IsAny<string>()))
                .ReturnsAsync(new[] { userDto });

            var users = await _userRepository.GetUsers();

            users.Should().BeEquivalentTo(new[]
            {
                new User(1, 18, "Test", "User", "M")
            });
        }

        [Test]
        public async Task GetUsers_GivenApiReturnsUsers_CachesResult()
        {
            var userDto = new UserDto { Age = 18, FirstName = "Test", Gender = "M", Id = 1, LastName = "User" };
            _apiClientMock
                .Setup(client => client.GetJsonAsync<IEnumerable<UserDto>>(It.IsAny<string>()))
                .ReturnsAsync(new[] { userDto });

            var users = await _userRepository.GetUsers();

            _cacheMock.Verify(
                cache => cache.Set(It.IsAny<object>(), users),
                Times.Once
            );
        }

        [Test]
        public async Task GetUsers_GivenCacheContainsUsers_ReturnsCachedResultWithoutMakingApiCall()
        {
            IEnumerable<User>? cachedUsers = new[] { new User(1, 18, "Test", "User", "M") };

            _cacheMock
                .Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedUsers))
                .Returns(true);

            var users = await _userRepository.GetUsers();

            users.Should().BeSameAs(cachedUsers);

            _apiClientMock.VerifyNoOtherCalls();
        }
    }
}