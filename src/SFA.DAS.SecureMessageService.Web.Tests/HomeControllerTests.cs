using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Web.Controllers;

namespace SFA.DAS.SecureMessageService.Web.Tests
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
            var controller = new HomeController(logger.Object);

            var result = controller.Index();

            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var actualViewResult = result as ViewResult;
            Assert.IsNotNull(actualViewResult);
            Assert.AreEqual("Index", actualViewResult.ViewName);
        }
    }
}