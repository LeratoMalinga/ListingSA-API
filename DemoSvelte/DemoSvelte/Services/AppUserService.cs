using DemoSvelte.Models;
using MongoDB.Driver;

namespace DemoSvelte.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IMongoCollection<AppUser> _appusers;

        public AppUserService(IPropertyListDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _appusers = database.GetCollection<AppUser>(settings.AppUserCollectionName);
        }
    }
}
