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

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<MessagesController>>();
            messageService = new Mock<IMessageService>();
        }

        [Test]
        public async Task Messages_SuccessfullySavesAValidForm()
        {
            var controller = new MessagesController(messageService.Object, logger.Object);
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();

            mockRequest.SetupGet(t => t.Form["TtlValue.Keys"]).Returns("1");
            mockRequest.SetupGet(t => t.Form["Message"]).Returns("testmessage");
            mockRequest.SetupGet(t => t.Scheme).Returns("https");
            mockRequest.SetupGet(t => t.Host).Returns(new HostString("localhost", 1234));

            messageService.Setup(c => c.Create("testmessage", 1)).ReturnsAsync("somekey1234");

            mockHttpContext.SetupGet(h => h.Request).Returns(mockRequest.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            var result = await controller.SaveMessage();

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ViewName, "ShowMessageUrl");
            var model = viewResult.Model as ShowMessageUrlViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Url, "https://localhost:1234/messages/somekey1234");
        }
    }
}