namespace App.Core
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
    }
}