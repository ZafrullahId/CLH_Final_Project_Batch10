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
    public class WalletController : ControllerBase
    {
        private readonly IWalletServices _walletServices;   
        public WalletController(IWalletServices walletServices)
        {
            _walletServices = walletServices;
        }
        [HttpPost("CreateWallet")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateWalletRequestModel model)
        {
            var wallet = await _walletServices.CreateWallet(model);
            if(wallet.Success == false)
            {
                return BadRequest(wallet);
            }
            return Ok(wallet);
        }
    }
}