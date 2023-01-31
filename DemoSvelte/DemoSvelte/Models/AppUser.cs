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

        //[BsonId]
       // [BsonRepresentation(BsonType.ObjectId)]
       // public string Id { get; set; } = string.Empty;
       // [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;
       // [BsonElement("Email")]
       //public string Email { get; set; } = string.Empty;
       // [BsonElement("Password")]
        //public string Password { get; set; } = string.Empty;
       // [BsonElement("UserName")]
        //public string Username { get; set; } = string.Empty;
       // [BsonElement("UserRole")]
        //public string UserRole { get; set; } = string.Empty;

    }
}
