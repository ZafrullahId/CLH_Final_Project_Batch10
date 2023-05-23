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
        private readonly IProductionRawMaterialService _productionRawMaterialService;
        public ProductionController(IProductionServices productionServices,IProductionRawMaterialService productionRawMaterialService)
        {
            _productionServices = productionServices;
            _productionRawMaterialService = productionRawMaterialService;
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
        [HttpGet("GetAprovedMonthlyProductions/{year}/{month}")]
        public async Task<IActionResult> GetAllAprovedByMonthAsync([FromRoute]int year,[FromRoute]int month)
        {
            var production = await _productionServices.GetAllAprovedProductionsByMonthAsync(year,month);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetApprovedProductions")]
        public async Task<IActionResult> GetAllAprovedByYearAsync()
        {
            var production = await _productionServices.GetAllAprovedProductionsAsync();
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetYearlyApprovedProductionsOnEachProduct")]
        public async Task<IActionResult> GetAllByYearAsync()
        {
            var production = await _productionServices.GetAllApprovedProductionsOnEachProductAsync();
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
        [HttpGet("GetAllPendingProductionsByMonth")]
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
        [HttpGet("GetByRawMaterialId/{id}")]
        public async Task<IActionResult> GetByGetByRawMaterialIdAsync(int id)
        {
             var productions = await _productionRawMaterialService.GetProductionByRwamaterialIdAsync(id);
            if(productions.Success == false)
            {
                return BadRequest(productions);
            }
            return Ok(productions);
        }
        [HttpPut("UpdateProduction/{id}")]
        public async Task<IActionResult> UpdateAsync([FromForm]UpdateProductionRequestModel model,[FromQuery]List<int> ids,[FromRoute] int id)
        {
            var production = await _productionServices.UpdateProductionAsync(id,model,ids);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpPut("ApproveProduction/{id}")]
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
        public async Task<IActionResult> RejectAsync([FromRoute] int id, [FromBody]RejectRequestRequestModel model)
        {
            var production = await _productionServices.RejectProduction(id, model);
            if(production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
        [HttpGet("GetAllProductions")]
        public async Task<IActionResult> GetAllProductionsAsync()
        {
            var productions = await _productionServices.GetAllProductionAsync();
            if (productions.Success == false)
            {
                return BadRequest(productions);
            }
            return Ok(productions);
        }
        [HttpGet("GetProduction/{id}")]
        public async Task<IActionResult> GetProductionAsync(int id)
        {
            var productions = await _productionServices.GetProductionById(id);
            if (productions.Success == false)
            {
                return BadRequest(productions);
            }
            return Ok(productions);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var production = await _productionServices.DeleteAsync(id);
            if (production.Success == false)
            {
                return BadRequest(production);
            }
            return Ok(production);
        }
    }
}