using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Project.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyControllerBase : ControllerBase
    {
        public MyControllerBase()
        {

        }
        [HttpGet]
        public IActionResult GetCurrentProtocol()
        {
            return Ok(HttpContext.Request.Host);
        }
    }
}