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
        private readonly IProductOrdersServices _productOrdersServices;
        public OrderController(IOrderServices orderServices,IProductOrdersServices productOrdersServices)
        {   
            _orderServices = orderServices;
            _productOrdersServices = productOrdersServices;
        }
        [HttpPost("CreateOrder/{userId}/{productId}")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateOrderRequestModel model,[FromRoute]int userId,[FromRoute]int productId)
        {
            var order = await _orderServices.CreateOrderAsync(model,userId,productId);
            if (order.Success == false)
            {
                return BadRequest(order);
            }
            return Ok(order);
        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var orders = await _productOrdersServices.Orders();
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GettAllAsync()
        {
            var orders = await _orderServices.GetAllDeleveredOrders();
            if(orders.Success == false)
            {
                return BadRequest(orders);
            }
            return Ok(orders);
        }
    }
}