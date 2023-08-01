using DemoSvelte.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

public class ChatMessageService
{
    private readonly IMongoCollection<ChatMessage> _chatMessages;

    public ChatMessageService(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _chatMessages = database.GetCollection<ChatMessage>(collectionName);
    }

    public List<ChatMessage> GetMessagesByCommunicationId(string communicationId)
    {
        return _chatMessages.Find(message => message.CommunicationId == communicationId).ToList();
    }
}
