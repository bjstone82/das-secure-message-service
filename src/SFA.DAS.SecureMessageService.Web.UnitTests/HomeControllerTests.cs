using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Web.Controllers;
using SFA.DAS.SecureMessageService.Web.Models;

namespace SFA.DAS.SecureMessageService.Web.UnitTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        protected Mock<ILogger<HomeController>> logger;
        protected Mock<IMessageService> messageService;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<HomeController>>();
            messageService = new Mock<IMessageService>();
        }

        [Test]
        public void Index_ReturnsExpectedViewResult()
        {
            // Arrange
            var controller = new HomeController(logger.Object, messageService.Object);

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
        public async Task Index_SuccessfullyRedirectsOnFormPost()
        {
            // Arrange
            var testMessage = "testmessage";
            var testTtl = 1;
            var testKey = "somekey1234";

            var controller = new HomeController(logger.Object, messageService.Object);

            messageService.Setup(c => c.Create(testMessage, testTtl)).ReturnsAsync(testKey);

            var indexViewModel = new IndexViewModel()
            {
                Message = testMessage,
                Ttl = testTtl
            };

            // Act
            var result = await controller.IndexSubmitMessage(indexViewModel);

            // Assert
            Assert.IsNotNull(result);
            var actualResult = result as RedirectToActionResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("ShareMessageUrl", actualResult.ActionName);
            Assert.AreEqual("Messages", actualResult.ControllerName);
            Assert.AreEqual(new RouteValueDictionary(new { key = testKey }), actualResult.RouteValues);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task Index_ReturnsBadRequestWhenNoMessage(string message)
        {
            // Arrange
            var testTtl = 1;
            var testKey = "somekey1234";

            var controller = new HomeController(logger.Object, messageService.Object);

            messageService.Setup(c => c.Create(message, testTtl)).ReturnsAsync(testKey);

            var indexViewModel = new IndexViewModel()
            {
                Message = message,
                Ttl = testTtl
            };

            // Act
            var result = await controller.IndexSubmitMessage(indexViewModel);

            // Assert
            var actualResult = result as BadRequestResult;
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Error_ReturnsExpectedViewResult()
        {
            // Arrange
            var controller = new HomeController(logger.Object, messageService.Object);

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