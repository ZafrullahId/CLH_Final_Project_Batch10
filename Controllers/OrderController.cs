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
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices,IProductOrdersServices productOrdersServices)
        {   
            _orderServices = orderServices;
        }
        [HttpPost("CreateOrder/{userId}")]
        public async Task<IActionResult> CreateAsync([FromBody]CreateOrderRequestModel model,[FromRoute]int userId)
        {
            var order = await _orderServices.CreateOrderAsync(model,userId);
            if (order.Success == false)
            {
                return BadRequest(order);
            }
            return Ok(order);
        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var orders = await _orderServices.GetAllOrders();
            if (orders.Success == false)
            {
                return BadRequest(orders);
            }
            return Ok(orders);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute]int id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            if (order.Success == false)
            {
                return BadRequest(order);
            }
            return Ok(order);
        }
        [HttpGet("GetByCustomerId/{id}")]
        public async Task<IActionResult> GetOrderByCustomerIdAsync([FromRoute]int id)
        {
            var order = await _orderServices.GetOrdersByCustomerIdAsync(id);
            if (order.Success == false)
            {
                return BadRequest(order);
            }
            return Ok(order);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatedOrders(int id)
        {
            var order = await _orderServices.UpdateOrder(id);
            if (order.Success == false)
            {
                return BadRequest(order);
            }
            return Ok(order);
        }
        [HttpGet("GetAllDeleveredOrders")]
        public async Task<IActionResult> GetAllDeleveredOrders()
        {
            var orders = await _orderServices.GetAllDeleveredOrders();
            if(orders.Success == false)
            {
                return BadRequest(orders);
            }
            return Ok(orders);
        }
        [HttpGet("GetAllUnDeleveredOrders")]
        public async Task<IActionResult> GetAllUnDeleveredOrders()
        {
            var orders = await _orderServices.GetAllUnDeleveredOrders();
            if(orders.Success == false)
            {
                return BadRequest(orders);
            }
            return Ok(orders);
        }
    }
}