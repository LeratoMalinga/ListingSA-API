using DemoSvelte.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PusherServer;
using System.Net;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
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
              new { message = dto.Message,username =dto.Username});

            return Ok(new string[] {} );  

        }
    }
}
