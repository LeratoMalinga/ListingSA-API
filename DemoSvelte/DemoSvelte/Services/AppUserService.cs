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
        public AppUser Create(AppUser appUser)
        {
            _appusers.InsertOne(appUser);
            return appUser;
        }
        public List<AppUser> Get()
        {
            return _appusers.Find(AppUser => true).ToList();
        }

        public AppUser Get(string id)
        {
            return _appusers.Find(appuser => appuser.Id == id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            _appusers.DeleteOne(appusers => appusers.Id == id);
        }

        public void Update(string id, AppUser appuser)
        {
            _appusers.ReplaceOne(appuser => appuser.Id == id, appuser);
        }
    }
}
