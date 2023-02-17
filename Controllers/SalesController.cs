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
    public class SalesController : ControllerBase
    {
        private readonly ISalesServices _salesServices;
        public SalesController(ISalesServices salesServices)
        {
            _salesServices = salesServices;
        }
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameAsync([FromQuery]string name)
        {
            var sales = await _salesServices.GetSalesByCustomerNameAsync(name);
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetByNameAndDate")]
        public async Task<IActionResult> GetByNameAndDateAsync([FromQuery]string name,[FromQuery]DateTime dateBought)
        {
            var sales = await _salesServices.GetSalesByCustomerNameAndDateAsync(name,dateBought);
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetSalesForTheMonth/{month}/{year}")]
        public async Task<IActionResult> GetSalesForTheMonthOnEachProductAsync(int month,int year)
        {
            var sales = await _salesServices.GetSalesForTheMonthOnEachProduct(month,year);
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetSalesForThYear/{year}")]
        public async Task<IActionResult> GetSalesForTheYearOnEachProductAsync(int year)
        {
            var sales = await _salesServices.GetSalesForTheYearOnEachProduct(year);
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetAllSales")]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _salesServices.GetAllSales();
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetThisYearSales")]
        public async Task<IActionResult> GetThisYearSales()
        {
            var sales = await _salesServices.GetSalesForThisYear();
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("GetThisMonthSales")]
        public async Task<IActionResult> GetThisMonthSales()
        {
            var sales = await _salesServices.GetSalesForThisMonth();
            if(sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("CalculateProfit")]
        public async Task<IActionResult> CalculateProfitAsync()
        {
            var profit = await _salesServices.CalculateThisMonthProfitAsync();
            if(profit.Success == false)
            {
                return BadRequest(profit);
            }
            return Ok(profit);
        }
        [HttpGet("CalculateProfit/{month}/{year}")]
        public async Task<IActionResult> CalculateProfitAsync(int month, int year)
        {
            var profit = await _salesServices.CalculateMonthlyProfitAsync(month,year);
            if(profit.Success == false)
            {
                return BadRequest(profit);
            }
            return Ok(profit);
        }
        [HttpGet("GetSalesByProductNameForTheMonth/{productId}/{month}/{year}")]
        public async Task<IActionResult> GetSalesByProductNameForTheMonth(int productId, int month, int year)
        {
            var sales = await _salesServices.GetSalesByProductNameForTheMonth(productId,month,year);
            if (sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);
        }
        [HttpGet("CalculateAllMonthlySales/{year}")]
        public async Task<IActionResult> CalculateAllMonthlySalesAsync(int year)
        {
            var sales = await _salesServices.CalculateAllMonthlySalesAsync(year);
            if (sales.Success == false)
            {
                return BadRequest(sales);
            }
            return Ok(sales);           
        }
        [HttpGet("CalculateAllMonthlyRawMaterial/{year}")]
        public async Task<IActionResult> CalculateAllMonthlyRawMaterialAsync(int year)
        {
            var rawMaterials = await _salesServices.CalculateAllMonthlyRawMaterialAsync(year);
            if (rawMaterials.Success == false)
            {
                return BadRequest(rawMaterials);
            }
            return Ok(rawMaterials);           
        }
    }
}