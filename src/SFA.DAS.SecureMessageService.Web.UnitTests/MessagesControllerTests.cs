using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Web.Controllers;

namespace SFA.DAS.SecureMessageService.Web.UnitTests
{
    public class MessagesControllerTests
    {
        protected Mock<ILogger<MessagesController>> logger;
        protected Mock<IMessageService> messageService;
        protected ControllerContext controllerContext;
        protected string testHttpScheme = "https";
        protected string testHostname = "localhost";
        protected int testPort = 1234;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<MessagesController>>();
            messageService = new Mock<IMessageService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(t => t.Scheme).Returns(testHttpScheme);
            mockRequest.SetupGet(t => t.Host).Returns(new HostString(testHostname, testPort));
            mockHttpContext.SetupGet(h => h.Request).Returns(mockRequest.Object);
            controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
        }

        [Test]
        public async Task Messages_SuccessfullySavesAValidForm()
        {
            // Arrange
            var testMessage = "testmessage";
            var testTtl = 1;
            var testKey = "somekey1234";

            var controller = new MessagesController(messageService.Object, logger.Object);

            messageService.Setup(c => c.Create(testMessage, testTtl)).ReturnsAsync(testKey);

            controller.ControllerContext = controllerContext;

            var indexViewModel = new IndexViewModel()
            {
                Message = testMessage,
                Ttl = testTtl
            };

            // Act
            var result = await controller.SaveMessage(indexViewModel);

            // Assert
            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ViewName, "ShowMessageUrl");
            var model = viewResult.Model as ShowMessageUrlViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Url, $"{testHttpScheme}://{testHostname}:{testPort}/messages/{testKey}");
        }
    }
}