using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Infrastructure
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache cache;

        public CacheRepository(IDistributedCache _cache)
        {
            cache = _cache;
        }

        public async Task SaveAsync(string key, string message, int ttl)
        {

            // Save message to cache using given ttl
            await cache.SetStringAsync(key, message, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(ttl)
            });
        }

        public async Task<string> RetrieveAsync(string key)
        {
            // Retrieve cached item
            var message = await cache.GetStringAsync(key);

            // Remove cached item
            await cache.RemoveAsync(key);

            return message == null ? default(String) : message;
        }

        public async Task<bool> TestAsync(string key)
        {

            // Retrieve but do not remove message
            var message = await cache.GetStringAsync(key);

            return String.IsNullOrEmpty(message) ? false : true;
        }

    }
}
