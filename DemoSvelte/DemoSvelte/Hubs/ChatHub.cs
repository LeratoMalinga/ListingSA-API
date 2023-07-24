using DemoSvelte.Dtos;
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
            HttpContext httpContext = Context.GetHttpContext();

            string receiver = httpContext.Request.Query["userid"];
            string sender = Context.User.Claims.FirstOrDefault().Value;

            Groups.AddToGroupAsync(Context.ConnectionId, sender);
            if (!string.IsNullOrEmpty(receiver))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, receiver);
            }

            return base.OnConnectedAsync();
        }

        public Task SendMessageToGroup(ChatMessageVM chatMessageVM)
        {
            var ChatMessage = new ChatMessage
            {
                Id = chatMessageVM.Id,
                CommunicationId = chatMessageVM.CommunicationId,
                Message= chatMessageVM.Message,
                Receiver= chatMessageVM.Receiver,
                Sender= chatMessageVM.Sender,
                UserName= chatMessageVM.UserName,
                Timestamp= chatMessageVM.Timestamp,
            };
            _chatMessages.InsertOne(ChatMessage);

            return Clients.Group(chatMessageVM.Receiver).SendAsync("ReceiveMessage", chatMessageVM.Sender, chatMessageVM.Message);
        }
    }
}
