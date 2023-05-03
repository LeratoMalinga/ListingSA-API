using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoSvelte.Models
{
    [BsonIgnoreExtraElements]
    public class NewsletterSubscriber
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; } = String.Empty;
        [BsonElement("Email")]
        public string? Email { get; set; }
        [BsonElement("Name")]
        public string? Name { get; set; }
    }
}
