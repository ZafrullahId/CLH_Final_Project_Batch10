using System.Threading.Tasks;
using Dansnom.Interface.Services;
using Dtos.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationCodeController : ControllerBase
    {
        private readonly IVerificationCodeService _verificationCodeService;
        public VerificationCodeController(IVerificationCodeService verificationCodeService)
        {
            _verificationCodeService = verificationCodeService;
        }
        [HttpGet("VerifyCode/{code}/{id}")]
        public async Task<IActionResult> VerifyCode([FromRoute] int code,int id)
        {
            var verifycode = await _verificationCodeService.VerifyCode(id,code);
            if(verifycode.Success == false)
            {
                return BadRequest(verifycode);
            }
            return Ok(verifycode);
        }
        [HttpPut("UpdateCode/{id}")]
        public async Task<IActionResult> UpdateCodeAsync([FromRoute] int id)
        {
            var code = await _verificationCodeService.UpdateVeryficationCodeAsync(id);
            if (code.Success == false)
            {
                return BadRequest(code);
            }
            return Ok(code);
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordRequestModel model)
        {
            var reset = await _verificationCodeService.SendForgetPasswordVerificationCode(model.Email);
            if (reset.Success == false)
            {
                return BadRequest(reset);
            }
            return Ok(reset);
        }
    }
}