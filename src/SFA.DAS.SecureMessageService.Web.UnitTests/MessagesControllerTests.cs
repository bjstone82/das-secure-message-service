using System;
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
using SFA.DAS.SecureMessageService.Web.Models;

namespace SFA.DAS.SecureMessageService.Web.UnitTests
{
    [TestFixture]
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
        public async Task Messages_SuccessfullyRetrievesMessageUrlWhenMessageExists()
        {
            // Arrange
            var testKey = "sometestkey5312";
            messageService.Setup(e => e.MessageExists(testKey)).ReturnsAsync(true);

            var controller = new MessagesController(messageService.Object, logger.Object);
            controller.ControllerContext = controllerContext;


            // Act
            var result = await controller.ShareMessageUrl(testKey);

            // Assert
            var actualResult = result as ViewResult;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(typeof(ShowMessageUrlViewModel), actualResult.Model.GetType());
            Assert.AreEqual(((ShowMessageUrlViewModel)actualResult.Model).Url, $"{testHttpScheme}://{testHostname}:{testPort}/messages/{testKey}");
        }
    }
}