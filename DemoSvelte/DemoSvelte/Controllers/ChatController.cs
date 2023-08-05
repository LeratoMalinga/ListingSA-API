using DemoSvelte.Dtos;
using DemoSvelte.Hubs;
using DemoSvelte.Models;
using DemoSvelte.Services;
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
        private readonly IChatMessageService _chatMessageService;
        public ChatController(IHubContext<ChatHub>  hubContext,IChatMessageService chatMessageService)
        {
            _hubContext = hubContext;
            _chatMessageService = chatMessageService;
        }

        [HttpGet("{userId}")]
        public ActionResult<ChatMessage> GetUserChatHistory(string userId)
        {
           var chatMessages = _chatMessageService.RequestChatHistory(userId);
           return Ok(chatMessages);

        }

    }
}
