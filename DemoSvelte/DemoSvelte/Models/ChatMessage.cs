using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoSvelte.Models
{
    [BsonIgnoreExtraElements]
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; } = String.Empty;
     
        [BsonElement("UserName")]
        public string? UserName { get; set; }
        [BsonElement("Sender")]
        public string? Sender { get; set; }
        [BsonElement("Reciever")]
        public string? Receiver { get; set; }
        [BsonElement("Message")]
        public string? Message { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime Timestamp { get; set; }
        [BsonElement("CommunicationId")]
        public string CommunicationId { get; set; }
        [BsonElement("IsPrivateMessage")]
        public Boolean IsPrivate { get; set; }

        [BsonElement("AppUser")]
        public virtual AppUser User { get; set; }
    }
}
