using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyResumeSiteBackEnd.Services;

using MyResumeSiteModels;

namespace MyResumeSiteBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<string> data)
        {
            if (!VariablesCore.ServerUrl.Contains("localhost"))
            {
                await _emailService.SendEmailToAdmin("Email From CV Site", string.Join(" | ", data));
            }
            return Ok();
        }
    }
}
