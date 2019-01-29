using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Core.Entities;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Core.Services;
using SFA.DAS.SecureMessageService.Core.IRepositories;
using System.Threading.Tasks;

namespace SFA.DAS.SecureMessageService.Core.UnitTests
{
    [TestFixture]
    public class MessageServiceTests
    {
        protected Mock<IProtectionRepository> protectionRepository;
        protected Mock<ICacheRepository> cacheRepository;

        [SetUp]
        public void Setup()
        {
            protectionRepository = new Mock<IProtectionRepository>();
            cacheRepository = new Mock<ICacheRepository>();
        }

        [Test]
        public async Task MessageService_CreateReturnsAValidKey()
        {
            // Arrange
            var unprotectedMessage = "testmessage";
            var protectedMessage = "xxxxxxxxx";
            var testTtl = 1;
            Guid key;

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            protectionRepository.Setup(c => c.Protect(unprotectedMessage)).Returns(protectedMessage);

            // Act
            var result = await service.Create(unprotectedMessage, testTtl);

            // Assert
            Assert.IsTrue(Guid.TryParse(result, out key));
        }

        [Test]
        public void MessageService_CreateDoesNotReturnAValidKey()
        {
            // Arrange
            var unprotectedMessage = "testmessage";
            var testTtl = 1;

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            protectionRepository.Setup(c => c.Protect(unprotectedMessage)).Throws<Exception>();

             // Assert
            Assert.ThrowsAsync<Exception>(async () => await service.Create(unprotectedMessage, testTtl));
        }

        [Test]
        public async Task MessageService_MessageExistsInCache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            cacheRepository.Setup(c => c.TestAsync(key)).ReturnsAsync(true);

            // Act
            var result = await service.MessageExists(key);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task MessageService_MessageDoesNotExistInCache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            cacheRepository.Setup(c => c.TestAsync(key)).ReturnsAsync(false);

            // Act
            var result = await service.MessageExists(key);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MessageService_MessageExistsInCacheThrows()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            cacheRepository.Setup(c => c.TestAsync(key)).Throws<Exception>();

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await service.MessageExists(key));
        }

        [Test]
        public async Task MessageService_RetrieveReturnsAValidMessage()
        {
            // Arrange
            var unprotectedMessage = "testmessage";
            var protectedMessage = "xxxxxxxxx";
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            cacheRepository.Setup(c => c.RetrieveAsync(key)).ReturnsAsync(protectedMessage);
            protectionRepository.Setup(c => c.Unprotect(protectedMessage)).Returns(unprotectedMessage);

            // Act
            var result = await service.Retrieve(key);

            // Assert
            Assert.AreEqual(unprotectedMessage, result);
        }

        [Test]
        public void MessageService_RetrieveThrows()
        {
            // Arrange
            var protectedMessage = "xxxxxxxxx";
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            var service = new MessageService(protectionRepository.Object, cacheRepository.Object);
            cacheRepository.Setup(c => c.RetrieveAsync(key)).ReturnsAsync(protectedMessage);
            protectionRepository.Setup(c => c.Unprotect(protectedMessage)).Throws<Exception>();

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await service.Retrieve(key));
        }
    }
}