using DemoSvelte.Dtos;
using DemoSvelte.Hubs;
using DemoSvelte.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PusherServer;
using System.Net;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IHubContext<ChatHub>  hubContext)
        {
            _hubContext = hubContext;
        }


        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] ChatMessageVM msg)
        {
            // Send the message to the group using the ChatHub
            await _hubContext.Clients.Group(msg.Receiver).SendAsync("ReceiveMessage", msg.Sender, msg.Message);

            return Ok();
        }


        [HttpPost("Message")]
        public async Task<ActionResult> Message(MessageDTO dto)
        {
            var options = new PusherOptions
            {
                Cluster = "ap2",
                Encrypted = true
            };

            var pusher = new Pusher(
              "1565284",
              "d4640bf31ce86bdd4b12",
              "59545c2d1d94cea97460",
              options);

            await pusher.TriggerAsync(
             "chat",
             "message",
             new { message = dto.Message, username = dto.Username });

            return Ok(new string[] { });

        }
    }
}
