using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace DemoSvelte.Models
{
    [BsonIgnoreExtraElements]
    [CollectionName("AppUsers")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;
    }
}
