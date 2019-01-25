using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SFA.DAS.SecureMessageService.Core.Entities;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Infrastructure
{
    public class AuditRepository : IAuditRepository
    {

        private readonly ConfigEntity config;
        private readonly String tableName;

        public AuditRepository(IOptions<ConfigEntity> _config)
        {
            config = _config.Value;
            tableName = "audit";
        }

        public async Task AuditMessageCreation(string key)
        {
            var storageAccount = CloudStorageAccount.Parse(config.StorageAccountConnectionString);

            // Create table client and table if not exists
            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            // Inject entity
            var entity = new CreateAuditEntity(key);

            var insertOperation = TableOperation.Insert(entity);

            await table.ExecuteAsync(insertOperation);
        }

        public async Task AuditMessageRetrieval(string key)
        {
            var storageAccount = CloudStorageAccount.Parse(config.StorageAccountConnectionString);

            // Create table client and table if not exists
            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            // Inject entity
            var entity = new RetrieveAuditEntity(key);

            var insertOperation = TableOperation.Insert(entity);

            await table.ExecuteAsync(insertOperation);
        }
    }
}
