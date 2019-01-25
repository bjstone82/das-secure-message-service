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
        private readonly ConfigEntity config;
        private readonly IProtectionRepository protectionRepository;
        private readonly ICacheRepository cacheRepository;

        private readonly IAuditRepository auditRepository;

        public MessageService(IProtectionRepository _protectionRepository, IOptions<ConfigEntity> _config, ICacheRepository _cacheRepository, IAuditRepository _auditRepository)
        {
            protectionRepository = _protectionRepository;
            config = _config.Value;
            cacheRepository = _cacheRepository;
            auditRepository = _auditRepository;
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

                // Audit message creation
                await auditRepository.AuditMessageCreation(key);

                // Return token to be used in uri
                return key;
            }
            catch (Exception e)
            {
                throw new Exception("Could not cache message.", e);
            }
        }

        public async Task<bool> MessageExists(string key)
        {
            try
            {

                // Test to see if the message key exists in the cache
                return await cacheRepository.TestAsync(key);
            }
            catch (Exception e)
            {
                throw new Exception("Could not validate message.", e);
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

                // Audit message retrievals
                await auditRepository.AuditMessageRetrieval(key);

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
