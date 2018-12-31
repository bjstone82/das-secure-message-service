using System.Threading.Tasks;

namespace SFA.DAS.SecureMessageService.Core.IRepositories
{
    public interface IProtectionRepository {
        Task<string> Protect(string message);
        Task<string> Unprotect(string message);
    }
}