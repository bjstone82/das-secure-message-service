using System;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Distributed;
using SFA.DAS.SecureMessageService.Core.IRepositories;
using SFA.DAS.SecureMessageService.Infrastructure.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System.Threading.Tasks;

namespace SFA.DAS.SecureMessageService.Infrastructure.UnitTests
{
    [TestFixture]
    public class ProtectionRepositoryTests
    {
        private Mock<IDasDataProtector> dataProtector;
        private ProtectionRepository repository;
        private string protectedMessage = "xxxxx";
        private string unprotectedMessage = "test message";

        [SetUp]
        public void Setup()
        {
            dataProtector = new Mock<IDasDataProtector>();
            repository = new ProtectionRepository(dataProtector.Object);
        }

        [Test]
        public void ProtectsAMessage()
        {
            // Arrange
            dataProtector.Setup(c => c.Protect(unprotectedMessage)).Returns(protectedMessage);

            // Act
            var result = repository.Protect(unprotectedMessage);

            // Assert
            Assert.AreEqual(protectedMessage, result);
        }

        [Test]
        public void FailsToProtectAMessageAndThrows()
        {
            // Arrange
            dataProtector.Setup(c => c.Protect(unprotectedMessage)).Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => repository.Protect(unprotectedMessage));
        }

        [Test]
        public void UnprotectsAMessage()
        {
            // Arrange
            dataProtector.Setup(c => c.Unprotect(protectedMessage)).Returns(unprotectedMessage);

            // Act
            var result = repository.Unprotect(protectedMessage);

            // Assert
            Assert.AreEqual(unprotectedMessage, result);
        }

        [Test]
        public void FailsToUnprotectAMessageAndThrows()
        {
            // Arrange
            dataProtector.Setup(c => c.Unprotect(protectedMessage)).Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => repository.Unprotect(protectedMessage));
        }

    }
}