using DemoSvelte.Dtos;
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
        private static Dictionary<string, string> _userConnectionIds = new Dictionary<string, string>();


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

        public async Task SendPrivateMessage(ChatMessageVM messagemodel)
        {
            var senderConnectionId = _userConnectionIds[messagemodel.Sender]; // Get the sender's connection ID from the dictionary
            var receiverUserId = Guid.Parse(messagemodel.Receiver);

            var sender = Users.FirstOrDefault(u => u.ConnectionId == senderConnectionId);
            var receiver = Users.FirstOrDefault(u => u.UserId == messagemodel.Receiver);

            // Load the chat history from the database for the current sender and receiver
            var chatHistory = _chatMessages
                .Find(x => (x.Sender == messagemodel.Sender && x.Receiver == messagemodel.Receiver) ||
                           (x.Sender == messagemodel.Receiver && x.Receiver == messagemodel.Sender))
                .ToList();

            if (receiver != null)
            {
                ChatMessage message = new ChatMessage
                {
                    Sender = messagemodel.Sender,
                    Receiver = messagemodel.Receiver,
                    UserName = messagemodel.UserName,
                    Message = messagemodel.Message,
                    Timestamp = DateTime.UtcNow,
                    User = _userManager.Users.FirstOrDefault(x => x.Id == Guid.Parse(messagemodel.Sender)),
                    CommunicationId = senderConnectionId,
                };

                // Save the message to the database for offline delivery
                _chatMessages.InsertOne(message);
                // Send the message to the receiver (if online) or save for later delivery (if offline)
                await Clients.Client(receiver.ConnectionId).SendAsync("ReceiveDM", messagemodel);
            }
            else
            {
                // Receiver is not online or not found, store the message for later delivery
                ChatMessage message = new ChatMessage
                {
                    Sender = messagemodel.Sender,
                    Receiver = messagemodel.Receiver,
                    UserName = messagemodel.UserName,
                    Message = messagemodel.Message,
                    Timestamp = DateTime.UtcNow,
                    User = _userManager.Users.FirstOrDefault(x => x.Id == Guid.Parse(messagemodel.Sender)),
                    CommunicationId = senderConnectionId,
                };

                // Save the message to the database for offline delivery
                _chatMessages.InsertOne(message);
            }

            // Send the chat history to the sender
            await Clients.Client(senderConnectionId).SendAsync("ReceiveChatHistory", chatHistory);
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
                _userConnectionIds[id] = cnID; // Add or update the connection ID in the dictionary
                await Groups.AddToGroupAsync(cnID, id);
            }
            else
            {
                existingUser.ConnectionId = cnID;
                _userConnectionIds[id] = cnID; // Add or update the connection ID in the dictionary
            }

            // Send the updated list of connected users to all clients
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
