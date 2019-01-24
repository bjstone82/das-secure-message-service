using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.SecureMessageService.Core.Entities
{

    public class AuditEntity : TableEntity
    {
        public AuditEntity()
        {
            this.PartitionKey = "SMS";
        }

        public string Key { get; set; }
    }

    public class CreateAuditEntity : AuditEntity
    {
        public CreateAuditEntity(string key)
        {
            this.RowKey = "MessageCreated";
            this.Key = key;
        }
    }

    public class RetrieveAuditEntity : AuditEntity
    {
        public RetrieveAuditEntity(string key)
        {
            this.RowKey = "MessageRetrieved";
            this.Key = key;
        }
    }
}