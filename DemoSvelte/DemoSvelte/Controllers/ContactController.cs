using DemoSvelte.Dtos;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public ContactController(IEmailService emailService) 
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public IActionResult Post(EmailDTO dTO)
        {
            _emailService.SendEmailAsync(dTO);

            return Ok();
        }
    }
}
