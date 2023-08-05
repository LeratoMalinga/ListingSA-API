using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> RequestChatHistory(string userId);
    }
}
