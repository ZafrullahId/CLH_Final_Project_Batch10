using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dansnom.Interface.Services;
using Dansnom.Dtos.RequestModel;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleService;
        public RoleController(IRoleServices roleServices)
        {
            _roleService = roleServices;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateRoleRequestModel request)
        {
           var isSuccessful = await _roleService.CreateRole(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }
         [HttpGet("GetAllRolesAsync")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var role = await _roleService.GetAllRoleAsync();
            if (role.Success == false)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetRoleAsync([FromRoute] int id)
        {
            var role = await _roleService.GetRoleByUserId(id);
            if (role.Success == false)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute]int id)
        {
            var role = await _roleService.DeleteRole(id);
            if(role.Success == false)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
    }
}