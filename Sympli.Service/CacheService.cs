using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Sympli.Services
{
    public class CacheService<TItem>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
        {
            TItem cacheEntry;

            if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
            {
                // Key not in cache, so get data.
                cacheEntry = await createItem();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetSize(1)//Size amount
                //Priority on removing when reaching size limit (memory pressure)
                .SetPriority(CacheItemPriority.High)
                // Remove from cache after this time, regardless of sliding expiration
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                // Save data in cache.
                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
