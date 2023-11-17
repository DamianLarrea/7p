using Microsoft.Extensions.Caching.Memory;

namespace App.Data.Caching
{
    // Don't need to implement IDisposable as the IMemoryCache should be managed by the IoC/DI container
    public class MemoryCache : ICache
    {
        private IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set<T>(object key, T entry, DateTimeOffset expiration) => _memoryCache.Set(key, entry, expiration);

        public bool TryGetValue<T>(object key, out T? value) => _memoryCache.TryGetValue(key, out value);
    }
}
