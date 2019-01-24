using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.SecureMessageService.Core.Entities
{
    public class CreateAuditEntity : TableEntity
    {
        public CreateAuditEntity(string key)
        {
            this.PartitionKey = "sms";
            this.RowKey = "Create";
            this.Key = key;
        }
        public string Key { get; set; }
    }

    public class RetrieveAuditEntity : TableEntity
    {
        public RetrieveAuditEntity(string key)
        {
            this.PartitionKey = "sms";
            this.RowKey = "Retrieve";
            this.Key = key;
        }
        public string Key { get; set; }
    }
}