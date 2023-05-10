using DemoSvelte.Dtos;

namespace DemoSvelte.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO request);
    }
}
