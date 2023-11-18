namespace App.Core
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserAsync(int id, CancellationToken token)
        {
            var users = await _userRepository.GetUsersAsync(token);

            return users.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersForAgeAsync(int age, CancellationToken token)
        {
            var users = await _userRepository.GetUsersAsync(token);

            return users.Where(x => x.Age == age).ToList();
        }

        public async Task<IDictionary<int, IEnumerable<User>>> GetUsersByAgeAsync(CancellationToken token)
        {
            var users = await _userRepository.GetUsersAsync(token);

            return users.GroupBy(u => u.Age).ToDictionary(grp => grp.Key, grp => grp.ToList() as IEnumerable<User>);
        }
    }
}
