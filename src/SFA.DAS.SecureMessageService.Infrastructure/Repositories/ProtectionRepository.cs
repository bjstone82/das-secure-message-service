using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Infrastructure
{
    public class ProtectionRepository : IProtectionRepository
    {
        private readonly IDataProtector dataProtector;

        public ProtectionRepository(IDataProtectionProvider _provider)
        {
            dataProtector = _provider.CreateProtector("SFA.DAS.SecureMessageService.Infrastructure.Repositories.ProtectionRepository");
        }

        public string Protect(string message)
        {
            try
            {
                return dataProtector.Protect(message);

            }
            catch (Exception e)
            {
                throw new Exception("Could not protect message", e);
            }
        }

        public string Unprotect(string message)
        {
            try
            {
                return dataProtector.Unprotect(message);
            }
            catch (Exception e)
            {
                throw new Exception("Could not unprotect message", e);
            }
        }
    }
}
