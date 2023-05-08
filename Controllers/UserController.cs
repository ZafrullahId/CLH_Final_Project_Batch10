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
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequestModel model)
        {
            var logging = await _userServices.Login(model);
            if(logging.Success == false)
            {
                return BadRequest(logging);
            }
            return Ok(logging);
        }
        [HttpGet("GetUsersByRole/{role}")]
        public async Task<IActionResult> GetUsersByRoleAsync([FromRoute]string role)
        {
            var users = await _userServices.GetUsersByRoleAsync(role);
            if(users.Success == false)
            {
                return BadRequest(users);
            }
            return Ok(users);
        }
        [HttpGet("GetUserByToken")]
        public async Task<IActionResult> GetUserByTokenAsync([FromQuery]string token)
        {
            var user = await _userServices.GetUserByTokenAsync(token);
            if (user.Success == false)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
    }
}