using DemoSvelte.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace DemoSvelte.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMongoCollection<ChatMessage> _chatMessages;

        public ChatHub(IPropertyListDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _chatMessages= database.GetCollection<ChatMessage>(settings.NewsletterSubcriber);
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public async Task NewMessage(string username, string message)
        {
            await Clients.All.SendAsync("messageReceived", username, message);

            // Save the message to MongoDB
            var chatMessage = new ChatMessage
            {
                UserName = username,
                Message = message
            };
            await _chatMessages.InsertOneAsync(chatMessage);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
