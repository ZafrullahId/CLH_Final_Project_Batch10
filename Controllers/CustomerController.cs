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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        public CustomerController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterAsync([FromForm]CreateCustomerRequestModel model)
        {
            var customer = await _customerServices.RegisterAsync(model);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromForm]UpdateCustomerRequestModel model, [FromRoute]int id)
        {
            var customer = await _customerServices.UpdateProfile(model,id);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var customer = await _customerServices.GetByidAsnc(id);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var customer = await _customerServices.DeleteAsync(id);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }
        [HttpGet("VerifyCode/{code}/{id}")]
        public async Task<IActionResult> VerifyCode([FromRoute] int code,int id)
        {
            var customer = await _customerServices.VerifyCode(id,code);
            if(customer.Success == false)
            {
                return BadRequest(customer);
            }
            return Ok(customer);
        }
    }
}