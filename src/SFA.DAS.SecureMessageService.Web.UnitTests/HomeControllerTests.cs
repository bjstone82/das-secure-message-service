using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Web.Controllers;

namespace SFA.DAS.SecureMessageService.Web.UnitTests
{
    public class HomeControllerTests
    {
        protected Mock<ILogger<HomeController>> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<HomeController>>();
        }

        [Test]
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var controller = new HomeController(logger.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var actualViewResult = result as ViewResult;
            Assert.IsNotNull(actualViewResult);
            Assert.AreEqual("Index", actualViewResult.ViewName);
        }
    }
}