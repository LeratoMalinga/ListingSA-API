using Microsoft.AspNetCore.SignalR;

namespace DemoSvelte.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }


        public async Task NewMessage(long username, string message) =>
        await Clients.All.SendAsync("messageReceived", username, message);

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }


    }
}
