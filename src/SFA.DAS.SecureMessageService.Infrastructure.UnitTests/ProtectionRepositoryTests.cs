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
        protected Mock<IDasDataProtector> dataProtector;
        protected string protectedMessage;
        protected string unprotectedMessage;

        [SetUp]
        public void Setup()
        {
            dataProtector = new Mock<IDasDataProtector>();

            protectedMessage = "xxxxxx";
            unprotectedMessage = "test message";
        }

        [Test]
        public void ProtectsAMessage()
        {
            // Arrange
            dataProtector.Setup(c => c.Protect(unprotectedMessage)).Returns(protectedMessage);
            var repository = new ProtectionRepository(dataProtector.Object);

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
            var repository = new ProtectionRepository(dataProtector.Object);

            // Assert
            Assert.Throws<Exception>(() => repository.Protect(unprotectedMessage));
        }

        [Test]
        public void UnprotectsAMessage()
        {
            // Arrange
            dataProtector.Setup(c => c.Unprotect(protectedMessage)).Returns(unprotectedMessage);
            var repository = new ProtectionRepository(dataProtector.Object);

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
            var repository = new ProtectionRepository(dataProtector.Object);

            // Assert
            Assert.Throws<Exception>(() => repository.Unprotect(protectedMessage));
        }

    }
}