using DemoSvelte.Models;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

public class ChatMessageService: IChatMessageService
{
    private readonly IMongoCollection<ChatMessage> _chatMessages;

    public ChatMessageService(IPropertyListDatabaseSettings settings,IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _chatMessages = database.GetCollection<ChatMessage>(settings.ChatMessage);
    }

    public async Task<List<ChatMessage>> RequestOpenChats(string userId)
    {

        var chatHistory = _chatMessages
        .Find(x => x.Sender == userId || x.Receiver == userId)
        .ToList();

        return chatHistory;
    }

    public async Task<List<ChatMessage>> RequestChatHistoryBetweenUsers(string userId, string otherUserId)
    {
        var chatHistory = _chatMessages
            .Find(x => (x.Sender == userId && x.Receiver == otherUserId) || (x.Sender == otherUserId && x.Receiver == userId))
            .ToList();

        return chatHistory;
    }

}
