using System.Threading.Tasks;

namespace SFA.DAS.SecureMessageService.Core.IRepositories
{
    public interface IAuditRepository {
        Task AuditMessageCreation(string key);
        Task AuditMessageRetrieval(string key);
    }
}