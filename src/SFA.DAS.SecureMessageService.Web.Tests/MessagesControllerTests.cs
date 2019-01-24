using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Web.Controllers;

namespace SFA.DAS.SecureMessageService.Web.Tests
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
    }
}