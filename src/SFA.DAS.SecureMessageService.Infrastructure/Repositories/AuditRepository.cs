using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using SFA.DAS.SecureMessageService.Core.Entities;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Infrastructure
{
    public class AuditRepository : IAuditRepository
    {

        private readonly IDistributedCache cache;

        public CacheRepository(IDistributedCache _cache)
        {
            cache = _cache;
        }

        public async Task AuditMessageCreation(string key)
        {

        }

        public async Task AuditMessageRetrieval(string key)
        {

        }
    }
}
