using System;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Distributed;
using SFA.DAS.SecureMessageService.Infrastructure.Repositories;
using System.Threading.Tasks;
using SFA.DAS.SecureMessageService.Core.IRepositories;

namespace SFA.DAS.SecureMessageService.Infrastructure.UnitTests
{
    [TestFixture]
    public class CacheRepositoryTests
    {
        private Mock<IDasDistributedCache> _cache;
        private CacheRepository _repository;

        [SetUp]
        public void Setup()
        {
            _cache = new Mock<IDasDistributedCache>();
        
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";
            var ttl = 1;
            

            var timeSpan = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(ttl)
            };
            _cache.Setup(c => c.SetStringAsync(key, message, timeSpan));
            _repository = new CacheRepository(_cache.Object);
            
        }

        [Test]
        public async Task Cache_RetrevieReturnsAMessage()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";

            _cache.Setup(c => c.GetStringAsync(key)).ReturnsAsync(message);

            // Act
            var result = await _repository.RetrieveAsync(key);

            // Assert
            Assert.AreEqual(message, result);
        }

        [Test]
        public async Task Cache_RetrevieDoesNotReturnAMessage()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            _cache.Setup(c => c.GetStringAsync(key)).ReturnsAsync("");

            // Act
            var result = await _repository.RetrieveAsync(key);

            // TODO: Need to test removal?

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public async Task Cache_TestIfMessageExistsInCache()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";
            var message = "test message";

            _cache.Setup(c => c.GetStringAsync(key)).ReturnsAsync(message);

            // Act
            var result = await _repository.TestAsync(key);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Cache_TestIfMessageDoesNotExistsInCache()
        {
            // Arrange
            var key = "24a8d272-0bd5-422d-80f1-09fc21dc7f7f";

            _cache.Setup(c => c.GetStringAsync(key)).ReturnsAsync(default(String));

            // Act
            var result = await _repository.TestAsync(key);

            // Assert
            Assert.IsFalse(result);
        }
    }

}