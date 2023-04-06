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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAddressAsync(int id)
        {
            var address = await _addressService.GetAddressAsync(id);
            if (address.Success == false)
            {
                return BadRequest(address);
            }
            return Ok(address);
        }
    }
}