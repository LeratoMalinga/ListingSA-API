﻿using DemoSvelte.Models;
using DemoSvelte.Models.ViewModels;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

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
                    Text = "<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <title></title>\r\n    <style>\r\n        table,\r\n        td,\r\n        div,\r\n        h1,\r\n        p {\r\n            font-family: Arial, sans-serif;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body style=\"margin:0;padding:0;\">\r\n    <table role=\"presentation\"\r\n           style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;\">\r\n        <tr>\r\n            <td align=\"center\" style=\"padding:0;\">\r\n                <table role=\"presentation\"\r\n                       style=\"width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;\">\r\n                    <tr>\r\n                        <td align=\"center\" style=\"padding:40px 0 0px 0;\">\r\n                            <img src=\"https://raw.githubusercontent.com/SaintLerato/soulsaint.github.io/main/Screenshot%202023-05-09%20180030.png\"\r\n                                 alt=\"\" width=\"200\" style=\"height:auto;display:block;\" />\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"center\">\r\n                            <h1 style=\"position:relative; top:10px; font-size:35px; color:black;font-weight: normal;\">\r\n                                Newsletter Subscription\r\n                            </h1>\r\n                            <div style=\"margin-top:35px;background-color:#8BD4C2;width: 100%; height: 12px;\"></div>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"padding:36px 30px 42px 30px;\">\r\n            " +
                    $"                <h1 style=\"font-size:20px\">Good day {newsletterSubscriber.Name},</h1>\r\n                            <h2 style=\"font-size:18px;color:#6AB4A8\">\r\n                                Thank you for subscribing to our Newsletter\r\n                                you will be getting these updates from us:\r\n                            </h2>\r\n                            <table role=\"presentation\"\r\n                                   style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">\r\n                                <tr>\r\n                                    <td style=\"padding:0;width:50%;\" align=\"left\">\r\n                                      News<br>\r\n                                    New Properies<br>\r\n                                        Promos<br>\r\n                                        Updates\r\n                                    </td>\r\n                                    <td style=\"padding:0;width:50%; color: #8C8C8C;\" align=\"right\">\r\n                                        2nd Floor, 1122<br>\r\n                                        Burnett St, Hatfield,<br> Pretoria, 0083\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <h2 style=\"font-size:15px;color:#6AB4A8\">\r\n                                No better Place to look for a place😁\r\n                                😉\r\n                            </h2>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"padding:30px;background:#9ED7CD;\">\r\n                            <table role=\"presentation\"\r\n                                   style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">\r\n                                <tr>\r\n                                    <td style=\"padding:0;width:50%;\" align=\"left\">\r\n                                        <p style=\"color:black\">\r\n                                            Regards<br>ListingSA<br /><br />Copyright &reg; 2022\r\n                                            ListingSA. All rights reserved.\r\n                                        </p>\r\n\r\n                                    </td>\r\n                                    <td style=\"padding:0;width:50%;\" align=\"right\">\r\n                                        <table role=\"presentation\"\r\n                                               style=\"border-collapse:collapse;border:0;border-spacing:0;\">\r\n                                            <tr>\r\n                                                <td style=\"padding:0 0 0 10px;width:38px;\">\r\n                                                    <a href=\"https://www.instagram.com/dr.ian.erasmus.inc/?hl=en\">\r\n                                                        <img src=\"https://user-images.githubusercontent.com/33877163/166319432-929a5b2c-390a-4207-a2e3-22414faf964e.png\"\r\n                                                             alt=\"ig\" width=\"38\"\r\n                                                             style=\"height:auto;display:block;border:0;\" />\r\n                                                    </a>\r\n                                                </td>\r\n                                                <td style=\"padding:0 0 0 10px;width:38px;\">\r\n                                                    <a href=\"https://web.facebook.com/IanErasmusDental/?_rdc=1&_rdr\">\r\n                                                        <img src=\"https://user-images.githubusercontent.com/33877163/166319764-80afd1ab-06c1-4d2d-8f3b-585fc261a4dd.png\"\r\n                                                             alt=\"Facebook\" width=\"38\"\r\n                                                             style=\"height:auto;display:block;border:0;\" />\r\n                                                    </a>\r\n                                                </td>\r\n                                            </tr>\r\n\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"center\" style=\"font-size:14px; padding:20px 0 5px 0;background:#64A89D;\">\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n\r\n</html>"
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
    


