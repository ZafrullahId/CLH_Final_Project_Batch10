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
    public class ProductionController : ControllerBase
    {
        private readonly IProductionServices _productionServices;   
        public ProductionController(IProductionServices productionServices)
        {
            _productionServices = productionServices;
        }
        [HttpPost("CreateProduction/{id}")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateProductionRequestModel model,[FromQuery]List<int> ids,[FromRoute]int id)
        {
            var production = await _productionServices.CreateProductionAsync(model,ids,id);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetAprovedMonthlyProductions/{month}")]
        public async Task<IActionResult> GetAllAprovedByMonthAsync([FromRoute]int month)
        {
            var production = await _productionServices.GetAllAprovedProductionsByMonthAsync(month);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetAprovedYearlyProductions/{year}")]
        public async Task<IActionResult> GetAllAprovedByYearAsync([FromRoute]int year)
        {
            var production = await _productionServices.GetAllAprovedProductionsByYearAsync(year);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetYearlyApprovedProductionsOnEachProduct/{year}")]
        public async Task<IActionResult> GetAllByYearAsync([FromRoute]int year)
        {
            var production = await _productionServices.GetAllApprovedProductionsOnEachProductByYearAsync(year);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetMonthlyApprovedProductionsOnEachProduct/{month}")]
        public async Task<IActionResult> GetAllByMonthlyAsync([FromRoute]int month)
        {
            var production = await _productionServices.GetAllApprovedProductionsOnEachProductByMonthAsync(month);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetAllPendingProductionsByMonth/{month}")]
        public async Task<IActionResult> GetAllPendingProductionsByMonthAsync()
        {
            var production = await _productionServices.GetAllPendingProductionsAsync();
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetProductions/{date}")]
        public async Task<IActionResult> GetAllByDateAsync([FromRoute]string date)
        {
            var production = await _productionServices.GetProductionsByDateAsync(date);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpPut("UpdateProduction/{id}")]
        public async Task<IActionResult> UpdateAsync([FromForm]UpdateProductionRequestModel model,[FromRoute] int id)
        {
            var production = await _productionServices.UpdateProductionAsync(id,model);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetByProductId/{id}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int id)
        {
            var production = await _productionServices.GetProductionsByProductIdAsync(id);
            if(production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpPut("AproveProduction/{id}")]
        public async Task<IActionResult> ApproveAsync([FromRoute] int id)
        {
            var production = await _productionServices.AproveProduction(id);
            if(production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpPut("RejectProduction/{id}")]
        public async Task<IActionResult> RejectAsync([FromRoute] int id)
        {
            var production = await _productionServices.RejectProduction(id);
            if(production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
    }
}