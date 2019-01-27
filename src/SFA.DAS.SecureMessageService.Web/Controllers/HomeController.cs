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
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;

        public HomeController(ILogger<HomeController> _logger, IMessageService _messageService)
        {
            logger = _logger;
            messageService = _messageService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index", new IndexViewModel());
        }

        [HttpPost("")]
        public  async Task<IActionResult> IndexSubmitMessage()
        {
            // Retrieve ttl value from the request
            var ttl = Convert.ToInt32(Request.Form["TtlValue.Keys"]);

            // Create the message and retrieve the url token
            var message = Request.Form["Message"];

            if (String.IsNullOrEmpty(message))
            {
                logger.LogError(1, "Message cannot be null");
                return new BadRequestResult();
            }

            var key = await messageService.Create(message, ttl);
            logger.LogInformation(1, $"Saving message: {key}");

            return RedirectToAction("ShareMessageUrl", "Messages", new {key = key} );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
        }
    }
}
