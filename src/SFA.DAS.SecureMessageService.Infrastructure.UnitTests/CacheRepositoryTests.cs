using System;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Distributed;
using SFA.DAS.SecureMessageService.Core.IRepositories;
using SFA.DAS.SecureMessageService.Infrastructure.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.SecureMessageService.Infrastructure.UnitTests
{
    [TestFixture]
    public class CacheRepositoryTests
    {
        protected Mock<IDistributedCache> cache;

        [SetUp]
        public void Setup()
        {
            cache = new Mock<IDistributedCache>();
        }

        [Test]
        public void Cache_SavesAMessageToTheCache()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";
            var ttl = 1;
            

            var timeSpan = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(ttl)
            };

            var repository = new CacheRepository(cache.Object);
            cache.Setup(c => c.SetStringAsync(key, message, timeSpan, new CancellationToken())).Verifiable();
        }

        [Test]
        public async Task Cache_RetrevieReturnsAMessage()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";

            cache.Setup(c => c.GetStringAsync(key, new CancellationToken())).ReturnsAsync(message);
            var repository = new CacheRepository(cache.Object);

            // Act
            var result = await repository.RetrieveAsync(key);

            // Assert
            Assert.AreEqual(message, result);
        }

        [Test]
        public async Task Cache_RetrevieDoesNotReturnAMessage()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            cache.Setup(c => c.GetStringAsync(key, new CancellationToken())).ReturnsAsync("");
            var repository = new CacheRepository(cache.Object);

            // Act
            var result = await repository.RetrieveAsync(key);

            // TODO: Need to test removal?

            // Assert
            Assert.AreEqual(default(String), result);
        }

        [Test]
        public async Task Cache_TestIfMessageExistsInCache()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";

            cache.Setup(c => c.GetStringAsync(key, new CancellationToken())).ReturnsAsync(message);
            var repository = new CacheRepository(cache.Object);

            // Act
            var result = await repository.TestAsync(key);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Cache_TestIfMessageDoesNotExistsInCache()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            cache.Setup(c => c.GetStringAsync(key, new CancellationToken())).ReturnsAsync(default(String));
            var repository = new CacheRepository(cache.Object);

            // Act
            var result = await repository.TestAsync(key);

            // Assert
            Assert.IsFalse(result);
        }
    }

}