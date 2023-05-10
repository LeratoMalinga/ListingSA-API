using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface INewsletterService
    {
        List<NewsletterSubscriber> Get();
        NewsletterSubscriber Get(string Id);
        NewsletterSubscriber Create(NewsletterSubscriber newsletterSubscriber);
    }
}
