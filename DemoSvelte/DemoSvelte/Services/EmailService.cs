using DemoSvelte.Dtos;
using DemoSvelte.Models;
using MimeKit;

namespace DemoSvelte.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailDTO request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ListingSA", "lmalinga05@gmail.com"));
            message.To.Add(new MailboxAddress(request.AgentName, request.To));
            message.Subject = "Welcome to Our Newsletter!";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"A new Tenant by the Name{request.Name} has shown interest in your property " + $"here is their contact details" + $"Email:{request.Email}" + $"ContactNumber:{request.Phone}" + $""

            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("lmalinga05@gmail.com", "udaqgdqoqbqkajld");
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
