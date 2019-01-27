using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Web.Models;

namespace SFA.DAS.SecureMessageService.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService messageService;
        private readonly ILogger logger;

        public MessagesController(IMessageService _messageService, ILogger<MessagesController> _logger)
        {
            messageService = _messageService;
            logger = _logger;
        }

        [HttpPost("share")]
        public async Task<IActionResult> SaveMessage()
        {

            // Retrieve ttl value from the request
            var ttl = Convert.ToInt32(Request.Form["TtlValue.Keys"]);

            // Create the message and retrieve the url token
            var message = Request.Form["Message"];

            if (String.IsNullOrEmpty(message))
            {
                var exceptionMessage = "Message cannot be null";
                logger.LogError(1, exceptionMessage);
                throw new Exception(exceptionMessage);
            }

            var key = await messageService.Create(message, ttl);
            var url = $"{Request.Scheme}://{Request.Host}/messages/{key}";

            logger.LogInformation(1, "Saving message with key {{key}}", key);

            var showMessageUrlViewModel = new ShowMessageUrlViewModel() { Url = url };
            return View("ShowMessageUrl", showMessageUrlViewModel);
        }

        [HttpGet("messages/{key}")]
        public async Task<IActionResult> ConfirmViewMessage(string key)
        {
            var messageExists = await messageService.MessageExists(key);
            ViewBag.MessageExists = messageExists;
            logger.LogInformation(1, "Message with key {{key}} exists: {{exists}}", key, messageExists.ToString());

            return View();
        }


        [HttpPost("messages/{key}")]
        public async Task<IActionResult> ViewMessage(string key)
        {

            logger.LogInformation(1, "Attempting to retrieve message with key {{key}}", key);
            var message = await messageService.Retrieve(key);
            logger.LogInformation(1, "Message with key {{key}} has been removed from cache", key);

            var viewMessageViewModel = new ViewMessageViewModel() { Message = message };
            return View(viewMessageViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
