using System.Threading.Tasks;

namespace SFA.DAS.SecureMessageService.Core.IServices
{
    public interface IMessageService
    {
        Task<string> Create(string message, int ttl);

        Task<string> Retrieve(string key);
    }
}