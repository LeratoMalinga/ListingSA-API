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

    public async Task<List<ChatMessage>> RequestChatHistory(string userId)
    {
       
        // Load the chat history from the database for the current user and chat ID
        var chatHistory = _chatMessages
            .Find(x => (x.Sender == userId))
            .ToList();

        return chatHistory;
    }


}
