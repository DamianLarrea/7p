
namespace App.Data.Caching
{
    public interface ICache
    {
        public void Set<T>(object key, T entry);

        public bool TryGetValue<T>(object key, out T? value);
    }
}
