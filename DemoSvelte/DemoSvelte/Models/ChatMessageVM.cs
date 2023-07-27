using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoSvelte.Models
{
    public class ChatMessageVM
    {
        public string Id { set; get; }
        public string User { get; set; }
        public string? UserName { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string CommunicationId { get; set; }
    }
}
