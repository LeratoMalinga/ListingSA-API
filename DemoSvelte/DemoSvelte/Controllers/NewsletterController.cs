using DemoSvelte.Models;
using DemoSvelte.Models.ViewModels;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterService _newsletterService;

        public NewsletterController(INewsletterService newsletterService)
        {
            _newsletterService = newsletterService;
        }

        [HttpGet("Getsubcribers")]
        public ActionResult<List<NewsletterSubscriber>> GetSubscribers()
        {
            return _newsletterService.Get();
        }
        [HttpPost("Subscribe")]
        public async Task<ActionResult<NewsletterSubscriber>> Post([FromBody] NewsLetterSubcriberVM subscriber)
        {
            // Validate subscriber information
            if (string.IsNullOrWhiteSpace(subscriber.Email))
            {
                return BadRequest("Email address is required.");
            }
            if (!IsValidEmail(subscriber.Email))
            {
                return BadRequest("Invalid email address.");
            }

            var newsletterSubscriber = new NewsletterSubscriber
            {
                Name = subscriber.Name,
                Email = subscriber.Email
            };

            try
            {
                // Add subscriber to database
                _newsletterService.Create(newsletterSubscriber);

                // Send welcome email to subscriber
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ListingSA", "lmalinga05@gmail.com"));
                message.To.Add(new MailboxAddress(newsletterSubscriber.Name, newsletterSubscriber.Email));
                message.Subject = "Welcome to Our Newsletter!";
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Thank you for subscribing to our newsletter!"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("lmalinga05@gmail.com", "udaqgdqoqbqkajld");
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }

                return CreatedAtAction(nameof(GetSubscribers), new { id = newsletterSubscriber.Id }, newsletterSubscriber);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
    


