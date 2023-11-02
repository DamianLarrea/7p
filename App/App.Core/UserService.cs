namespace App.Core
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUser(int id)
        {
            var users = await _userRepository.GetUsers();

            return users.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersForAge(int age)
        {
            var users = await _userRepository.GetUsers();

            return users.Where(x => x.Age == age).ToList();
        }

        public async Task<IDictionary<int, int>> GetNumberOfUsersByAge()
        {
            var users = await _userRepository.GetUsers();

            return users.GroupBy(u => u.Age).ToDictionary(grp => grp.Key, grp => grp.Count());
        }
    }
}
