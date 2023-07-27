﻿using DemoSvelte.Dtos;
using DemoSvelte.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace DemoSvelte.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMongoCollection<ChatMessage> _chatMessages;
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(IPropertyListDatabaseSettings settings, IMongoClient mongoClient, UserManager<AppUser> userManager)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _chatMessages= database.GetCollection<ChatMessage>(settings.ChatMessage);
            _userManager = userManager;
        }

        public class UserConnection
        {
            public string UserId { get; set; }
            public string ConnectionId { get; set; }
            public string FullName { get; set; }
            public string Username { get; set; }
        }

        static IList<UserConnection> Users = new List<UserConnection>();
        public async Task SendMessageToUser(ChatMessageVM messagemodel)
        {
            var receiverGroup = messagemodel.Receiver; 
            var userid = Guid.Parse(messagemodel.User);

            var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);

            ChatMessage message = new ChatMessage
            {
                Sender = messagemodel.Sender,
                Receiver = messagemodel.Receiver,
                UserName = messagemodel.UserName,
                Message = messagemodel.Message,
                Timestamp = DateTime.UtcNow,
                User = user,
                CommunicationId = this.Context.ConnectionId,
            };

            _chatMessages.InsertOne(message);

            await Clients.Group(receiverGroup).SendAsync("ReceiveDM", message); 
        }
   
        public async Task PublishUserOnConnect(string id, string fullname, string username)
        {
            
            var existingUser = Users.FirstOrDefault(x => x.UserId == id);
            var cnID = Context.ConnectionId;
            UserConnection user = new UserConnection
            {
                UserId = id,
                ConnectionId = cnID,
                FullName = fullname,
                Username = username
            };

            if (existingUser == null)
            {
                Users.Add(user);
                await Groups.AddToGroupAsync(cnID, id);
            }
            else
            {
                existingUser.ConnectionId = cnID;
            }

            await Clients.All.SendAsync("BroadcastUserOnConnect", Users);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var user = Users.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user != null)
            {
                Users.Remove(user);
                await Groups.RemoveFromGroupAsync(connectionId, user.UserId); 
                await Clients.All.SendAsync("BroadcastUserOnDisconnect", Users);
            }

            await base.OnDisconnectedAsync(exception);
        }   
    }
}
