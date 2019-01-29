using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SFA.DAS.SecureMessageService.Core.IRepositories
{
    public interface IDasDistributedCache
    {
        Task SetStringAsync(string key, string message, DistributedCacheEntryOptions distributedCacheEntryOptions);
        Task<string> GetStringAsync(string key);
        Task RemoveAsync(string key);
    }
}