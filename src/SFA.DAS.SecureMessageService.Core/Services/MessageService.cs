using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.SecureMessageService.Core.Entities;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly SharedConfig config;
        private readonly IProtectionRepository protectionRepository;
        private readonly ICacheRepository cacheRepository;

        public MessageService(IProtectionRepository _protectionRepository, IOptions<SharedConfig> _config, ICacheRepository _cacheRepository)
        {
            protectionRepository = _protectionRepository;
            config = _config.Value;
            cacheRepository = _cacheRepository;
        }

        public async Task<string> Create(string message, int ttl)
        {
            try
            {
                // Generate storage key
                var key = Guid.NewGuid().ToString();

                // Protect string
                var protectedMessage = await protectionRepository.Protect(message);

                // Save string to cache
                await cacheRepository.SaveAsync(key, protectedMessage, ttl);

                // Return token to be used in uri
                return key;
            }
            catch (Exception e)
            {
                throw new Exception("Could not cache message.", e);
            }
        }

        public async Task<string> Retrieve(string key)
        {
            try
            {
                // Retrieve protected string from cache
                var protectedMessage = await cacheRepository.RetrieveAsync(key);

                // Encrypt string
                var unprotectedMessage = await protectionRepository.Unprotect(protectedMessage);

                // Return the unprotected message
                return unprotectedMessage;
            }
            catch (Exception e)
            {
                throw new Exception("Could not retrieve message.", e);
            }
        }
    }
}