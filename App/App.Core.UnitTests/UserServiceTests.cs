
using FluentAssertions;
using Moq;

namespace App.Core.UnitTests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        #region GetUser tests

        [Test]
        public async Task GetUser_PropagatesException_WhenRepositoryThrowsException()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsers()).ThrowsAsync(new TestException());
            
            var getUserAction = () => _userService.GetUser(0);

            await getUserAction.Should().ThrowExactlyAsync<TestException>();
        }

        [Test]
        public async Task GetUser_ReturnsNull_WhenUserDoesNotExist()
        {
            var user = await _userService.GetUser(0);

            user.Should().BeNull();
        }

        [Test]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            var user = new User(1, 18, "Test", "User", "M");

            _userRepositoryMock.Setup(repo => repo.GetUsers()).ReturnsAsync(new[] { user });

            var result = await _userService.GetUser(1);

            result.Should().Be(user);
        }

        #endregion

        #region GetUsersForAge tests

        [Test]
        public async Task GetUsersForAge_PropagateException_WhenRepositoryThrows()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsers()).ThrowsAsync(new TestException());

            var getUserAction = () => _userService.GetUsersForAge(0);

            await getUserAction.Should().ThrowExactlyAsync<TestException>();
        }

        [Test]
        public async Task GetUsersForAge_ReturnsEmptyList_WhenNoUsersForGivenAge()
        {
            var users = new List<User>()
            {
                new User(1, 20, "Test", "User1", "M"),
                new User(2, 21, "Test", "User2", "F")
            };

            _userRepositoryMock.Setup(repo => repo.GetUsers()).ReturnsAsync(users);

            var result = await _userService.GetUsersForAge(18);

            result.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        public async Task GetUsersForAge_ReturnsExpectedUsers_WhenUsersForGivenAgeExist()
        {
            var users = new List<User>()
            {
                new User(1, 18, "Test", "User1", "M"),
                new User(2, 18, "Test", "User2", "F")
            };

            _userRepositoryMock.Setup(repo => repo.GetUsers()).ReturnsAsync(users);

            var result = await _userService.GetUsersForAge(18);

            result.Should().BeEquivalentTo(users);
        }

        #endregion

        #region GetUsersByAge tests

        [Test]
        public async Task GetUsersByAge_PropagatesException_WhenRepositoryThrowsException()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsers()).ThrowsAsync(new TestException());

            var getUserAction = () => _userService.GetUsersByAge();

            await getUserAction.Should().ThrowExactlyAsync<TestException>();
        }

        [Test]
        public async Task GetUsersByAge_ReturnsEmptyDictionary_WhenNoUsersExist()
        {
            var result = await _userService.GetUsersByAge();

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetUsersByAge_ReturnsExpectedResult_WhenUsersExist()
        {
            var user1 = new User(1, 20, "Test", "User1", "M");
            var user2 = new User(2, 20, "Test", "User2", "F");
            var user3 = new User(3, 21, "Test", "User3", "F");

            var users = new [] { user1, user2, user3 };

            _userRepositoryMock.Setup(repo => repo.GetUsers()).ReturnsAsync(users);

            var result = await _userService.GetUsersByAge();

            var expected = new Dictionary<int, IEnumerable<User>>
            {
                { 20, new [] { user1, user2 } },
                { 21, new [] { user3 } }
            };

            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region Helper classes

        private class TestException: Exception { }

        #endregion
    }
}