using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Web.Controllers;
using SFA.DAS.SecureMessageService.Web.Models;

namespace SFA.DAS.SecureMessageService.Web.UnitTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        protected Mock<ILogger<HomeController>> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<HomeController>>();
        }

        [Test]
        public void Index_ReturnsExpectedViewResult()
        {
            // Arrange
            var controller = new HomeController(logger.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var actualViewResult = result as ViewResult;
            Assert.IsNotNull(actualViewResult);
            Assert.AreEqual(actualViewResult.Model.GetType(), typeof(IndexViewModel));
            Assert.AreEqual("Index", actualViewResult.ViewName);
        }

        [Test]
        public void Error_ReturnsExpectedViewResult()
        {
            // Arrange
            var controller = new HomeController(logger.Object);

            // Act
            var result = controller.Error();

            // Assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var actualViewResult = result as ViewResult;
            Assert.IsNotNull(actualViewResult);
            Assert.AreEqual(actualViewResult.Model.GetType(), typeof(ErrorViewModel));
            Assert.AreEqual("Error", actualViewResult.ViewName);
        }
    }
}