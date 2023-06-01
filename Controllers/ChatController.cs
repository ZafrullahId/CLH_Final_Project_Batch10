using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dansnom.Interface.Services;
using Dansnom.Dtos.RequestModel;
using Microsoft.Extensions.Configuration;
using Dansnom.Auth;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IJWTAuthenticationManager _tokenService;
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService, IJWTAuthenticationManager tokenService, IConfiguration config)
        {
            _chatService = chatService;
            _tokenService = tokenService;
            _config = config;
        }
        [HttpPost("CreateChat/{id}/{recieverId}")]
        public async Task<IActionResult> CreateChat([FromBody]CreateChatRequestModel model, [FromRoute]int id, [FromRoute]int recieverId)
        {
            string token = Request.Headers["Authorization"];
            string extractedToken = token.Substring(7);
            var isValid = _tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), extractedToken);
            if (!isValid)
            {
                return Unauthorized();
            }
            var chat = await _chatService.CreateChat(model, id, recieverId);
            if (chat.Success == false)
            {
                return BadRequest(chat);
            }
            return Ok(chat);
        }
        [HttpGet("Get/{senderId}/{recieverId}")]
        public async Task<IActionResult> GetChatFromASenderAsync([FromRoute]int senderId,[FromRoute] int recieverId)
        {
             var chat = await _chatService.GetChatFromASenderAsync(senderId, recieverId);
            if (chat.Success == false)
            {
                return BadRequest(chat);
            }
            return Ok(chat);
        }
        [HttpPut("MarkAllAsRead/{senderId}/{recieverId}")]
        public async Task<IActionResult> MarkAllAsRead([FromRoute]int senderId,[FromRoute] int recieverId)
        {
            var response = await _chatService.MarkAllChatsAsReadAsync(senderId,recieverId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllUnSeenChatAsync/{recieverId}")]
        public async Task<IActionResult> GetAllUnseenChats([FromRoute] int recieverId)
        {
            var response = await _chatService.GetAllUnSeenChatAsync(recieverId);
             if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}