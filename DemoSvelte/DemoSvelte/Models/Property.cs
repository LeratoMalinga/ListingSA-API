using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace DemoSvelte.Models
{
    [BsonIgnoreExtraElements]
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; } = String.Empty;
        [BsonElement("PropertyName")]
        public string Name { set; get; } = String.Empty;
        [BsonElement("Description")]
        public string Description { set; get; } = String.Empty;

        [BsonElement("Province")]
        public string Province { set; get; } = String.Empty;
        [BsonElement("City")]
        public string City { set; get; } = String.Empty;

        [BsonElement("Suburb")]
        public string Suburb { set; get; } = String.Empty;

        [BsonElement("Price")]
        public string Price { set; get; } = String.Empty;
        [BsonElement("Address")]
        public string Address { set; get; } = String.Empty;

        [BsonElement("Type")]
        public string Type { set; get; } = String.Empty;
        [BsonElement("Available")]
        public bool IsAvaliable { set; get; }
        [BsonElement("ImageBase64")]
        public string ImageBase64 { get; set; } = String.Empty;
        [BsonElement("AppUser")]
        public AppUser? AppUser { set; get; }
    }

}
