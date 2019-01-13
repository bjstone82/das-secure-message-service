using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.SecureMessageService.Core.IServices;
using SFA.DAS.SecureMessageService.Web.Models;

namespace SFA.DAS.SecureMessageService.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService messageService;
        public MessagesController(IMessageService _messageService)
        {
            messageService = _messageService;
        }

        [HttpPost("share")]
        public async Task<IActionResult> SaveMessage()
        {

            // Retrieve ttl value from the request
            var ttl = Convert.ToInt32(Request.Form["TtlValue.Keys"]);

            // Create the message and retrieve the url token
            var message = Request.Form["Message"];

            if (String.IsNullOrEmpty(message)){
                throw new Exception("Message cannot be null");
            }

            var key = await messageService.Create(message, ttl );
            var url = $"{Request.Scheme}://{Request.Host}/messages/{key}";

            var showMessageUrlViewModel = new ShowMessageUrlViewModel() { Url = url };
            return View("ShowMessageUrl",showMessageUrlViewModel);
        }

        [HttpGet("messages/{key}")]
        public async Task<IActionResult> ViewMessage(string key)
        {
            // Retrieve the protected message from the cache
            var message = await messageService.Retrieve(key);

            var viewMessageViewModel = new ViewMessageViewModel()  {Message = message };
            return View(viewMessageViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
