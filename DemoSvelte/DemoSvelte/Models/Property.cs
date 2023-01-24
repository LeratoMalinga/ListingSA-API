using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace DemoSvelte.Models
{
    [BsonIgnoreExtraElements]
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string  Id {set; get; } =String.Empty;
        [BsonElement("Name")]
        public string Name { set; get; } = String.Empty;
        [BsonElement("Description")]
        public string Description { set; get; } = String.Empty;

        [BsonElement("Price")]
        public double? Price { set; get; }
        [BsonElement("Type")]
        public string Type { set; get; } = String.Empty;
        [BsonElement("Available")]
        public bool IsAvaliable { set; get; }
        [BsonElement("Picture")]
        public string Picture { get ; set; } = String.Empty;
        [BsonElement("AppUser")]
        public AppUser? AppUser { set; get; }
    }

}
