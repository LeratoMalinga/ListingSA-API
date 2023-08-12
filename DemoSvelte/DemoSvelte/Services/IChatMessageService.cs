using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> RequestOpenChats(string userId);
        Task<List<ChatMessage>> RequestChatHistoryBetweenUsers(string userId, string otherUserId);
    }
}
