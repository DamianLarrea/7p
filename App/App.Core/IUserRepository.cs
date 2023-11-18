namespace App.Core
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsersAsync(CancellationToken token);
    }
}