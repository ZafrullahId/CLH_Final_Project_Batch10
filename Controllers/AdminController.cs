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
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [HttpPost("RegisterAdmin")]
            public async Task<IActionResult> RegisterAdmin([FromForm]CreateAdminRequestModel request)
            {
                var isSuccessful = await _adminServices.AddAdmin(request);
                if (isSuccessful.Success == false)
                {
                    return BadRequest(isSuccessful);
                }
                return Ok(isSuccessful);
            }
            [HttpGet("GetAllAdmins")]
            public async Task<IActionResult> GetAllAdmins()
            {
                var admins = await _adminServices.GetAllAdmins();
                if(admins.Success == false)
                {
                    return BadRequest(admins);
                }
                return Ok(admins);
            }
            [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromForm]UpdateAdminRequestModel model, [FromRoute]int id)
        {
            var customer = await _adminServices.UpdateProfile(model,id);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }

            [HttpGet("Get/{id}")]
            public async Task<IActionResult> GetAdminAsync([FromRoute] int id)
            {
                var Admin = await _adminServices.FindAdminAsync(id);
                if (Admin.Success == false)
                {
                    return BadRequest(Admin);
                }
                return Ok(Admin);
            }
            [HttpDelete("Delete/{id}")]
            public async Task<IActionResult> DeleteAsync([FromRoute] int id)
            {
                var Admin = await _adminServices.DeleteAdmin(id);
                if (Admin.Success == false)
                {
                    return BadRequest(Admin);
                }
                return Ok(Admin);
            }
    }
}