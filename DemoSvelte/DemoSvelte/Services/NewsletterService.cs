using DemoSvelte.Models;
using MongoDB.Driver;

namespace DemoSvelte.Services
{
    public class NewsletterService:INewsletterService
    {
        private readonly IMongoCollection<NewsletterSubscriber> _subscribers;
        public NewsletterService(IPropertyListDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _subscribers = database.GetCollection<NewsletterSubscriber>(settings.NewsletterSubcriber);
        }

        public NewsletterSubscriber Create(NewsletterSubscriber subcriber)
        {
            _subscribers.InsertOne(subcriber);
            return subcriber;
        }
        public List<NewsletterSubscriber> Get()
        {
            return _subscribers.Find(Subscriber => true).ToList();
        }

        public NewsletterSubscriber Get(string id)
        {
            return _subscribers.Find(subscriber => subscriber.Id == id).FirstOrDefault();
        }

    }
}
