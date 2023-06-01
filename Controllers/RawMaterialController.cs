using System.Net;
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
    public class RawMaterialController : ControllerBase
    {
        private readonly IRwavMaterialServuce _rawMaterialServuce;
        private readonly IConfiguration _config;
        private readonly IJWTAuthenticationManager _tokenService;
        public RawMaterialController(IRwavMaterialServuce rwavMaterialServuce, IJWTAuthenticationManager tokenService, IConfiguration config)
        {
            _rawMaterialServuce = rwavMaterialServuce;
            _tokenService = tokenService;
            _config = config;
        }
        [HttpPost("CreateRawMaterial/{managerId}")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateRawMaterialRequestModel model, [FromRoute] int managerId)
        {
            var RawMaterial = await _rawMaterialServuce.CreateRawMaterial(model, managerId);
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetRawAsync/{id}")]
        public async Task<IActionResult> GetRawAsync([FromRoute] int id)
        {
            var expense = await _rawMaterialServuce.GetRawAsync(id);
            if (expense.Success == false)
            {
                return BadRequest(expense);
            }
            return Ok(expense);
        }
        [HttpGet("GetAllAprovedMonthlyRawMaterial/{month}/{year}")]
        public async Task<IActionResult> GetAllAprovedMonthlyRawMaterialOnEachProductAsync([FromRoute] int month, int year)
        {
            var RawMaterial = await _rawMaterialServuce.GetAllAprovedRawMaterialsForTheMonthAsync(month, year);
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllRejectedMonthlyRawMaterial/{month}")]
        public async Task<IActionResult> GetAllUnAprovedMonthlyRawMaterialOnEachProductAsync([FromRoute] int month)
        {
            var RawMaterial = await _rawMaterialServuce.GetAllRejectedRawMaterialForTheMonthAsync(month);
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllAprovedYearlyRawMaterial/{year}")]
        public async Task<IActionResult> GetAllAprovedYearlyRawMaterialOnEachProductAsync([FromRoute] int year)
        {
            var RawMaterial = await _rawMaterialServuce.GetAllAprovedRawMateralsForTheYear(year);
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllApprovedRawMaterial")]
        public async Task<IActionResult> GetAllApprovedRawMaterialAsync()
        {
            var RawMaterial = await _rawMaterialServuce.GetAllApprovedRawMaterialAsync();
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllRejectedYearlyRawMaterial/{year}")]
        public async Task<IActionResult> GetAllUnAprovedYearlyRawMaterialOnEachProductAsync([FromRoute] int year)
        {
            var RawMaterial = await _rawMaterialServuce.GetAllRejectedRawMaterialForTheYearAsync(year);
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllPendingRawMaterial")]
        public async Task<IActionResult> GetAllPendingRawMaterialsync()
        {
            var RawMaterial = await _rawMaterialServuce.GetAllPendingRawMaterial();
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAllRawMaterials")]
        public async Task<IActionResult> GetAllRawMaterials()
        {
            var RawMaterial = await _rawMaterialServuce.GetAllRawMaterials();
            if (RawMaterial.Success == false)
            {
                return BadRequest(RawMaterial);
            }
            return Ok(RawMaterial);
        }
        [HttpGet("GetAvailableRawmaterails")]
        public async Task<IActionResult> GetGetAvailableRawmaterailsAsync()
        {
            var rawmaterials = await _rawMaterialServuce.GetAvailableRawMaterialsAsync();
            if (rawmaterials.Success == false)
            {
                return BadRequest(rawmaterials);
            }
            return Ok(rawmaterials);
        }

        [HttpGet("CalculateCostOfRawMaterialsForTheMonth")]
        public async Task<IActionResult> CalculateCostOfRawMaterialsForTheMonth()
        {
            var cost = await _rawMaterialServuce.CalculateRawMaterialCostForTheMonth();
            if (cost.Success == false)
            {
                return BadRequest(cost);
            }
            return Ok(cost);
        }
        [HttpGet("CalculateCostOfRawMaterialsForTheYear")]
        public async Task<IActionResult> CalculateCostOfRawMaterialsForThYear()
        {
            var cost = await _rawMaterialServuce.CalculateRawMaterialCostForThYear();
            if (cost.Success == false)
            {
                return BadRequest(cost);
            }
            return Ok(cost);
        }
        [HttpPut("UpdateRawMaterial/{id}")]
        public async Task<IActionResult> UpdateRawMaterial(int id, [FromForm] UpdateRawMaterialRequestModel model)
        {
            var raw = await _rawMaterialServuce.UpdateRawMaterialRequestAsync(id, model);
            if (raw.Success == false)
            {
                return BadRequest(raw);
            }
            return Ok(raw);
        }
        [HttpPut("ApproveRawMaterial/{id}")]
        public async Task<IActionResult> ApproveRawMaterialAsync([FromRoute] int id)
        {
            string token = Request.Headers["Authorization"];
            string extractedToken = token.Substring(7);
            var isValid = _tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), extractedToken);
            if (!isValid)
            {
                return Unauthorized();
            }
            var expense = await _rawMaterialServuce.ApproveRawMaterialAsync(id);
            if (expense.Success == false)
            {
                return BadRequest(expense);
            }
            return Ok(expense);
        }
        [HttpPut("RejectRawMaterial/{id}")]
        public async Task<IActionResult> RejectRawMaterialAsync([FromRoute] int id, [FromBody] RejectRequestRequestModel model)
        {
            string token = Request.Headers["Authorization"];
            string extractedToken = token.Substring(7);
            var isValid = _tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), extractedToken);
            if (!isValid)
            {
                return Unauthorized();
            }
            var expense = await _rawMaterialServuce.RejectRawMaterialAsync(id, model);
            if (expense.Success == false)
            {
                return BadRequest(expense);
            }
            return Ok(expense);
        }
        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> DeleteRequestAsync([FromRoute] int id)
        {
            var request = await _rawMaterialServuce.DeleteRawMaterialRequestAsync(id);
            if (request.Success == false)
            {
                return BadRequest(request);
            }
            return Ok(request);
        }
    }
}