using App.Data.Options;
using Microsoft.Extensions.Caching.Memory;

namespace App.Data.Caching
{
    // Don't need to implement IDisposable as the IMemoryCache should be managed by the IoC/DI container
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        private readonly CacheOptions _cacheOptions;

        public MemoryCache(IMemoryCache memoryCache, CacheOptions cacheOptions)
        {
            _memoryCache = memoryCache;
            _cacheOptions = cacheOptions;
        }

        public void Set<T>(object key, T entry) => _memoryCache.Set(key, entry, DateTime.UtcNow.AddSeconds(_cacheOptions.TimeoutInSeconds));

        public bool TryGetValue<T>(object key, out T? value) => _memoryCache.TryGetValue(key, out value);
    }
}
