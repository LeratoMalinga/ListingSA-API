using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface IChatMessageSertvice
    {
        List<ChatMessage> GetMessagesByCommunicationId(string communicationId);
    }
}
