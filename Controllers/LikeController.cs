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
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }
        [HttpPost("CreateLike")]
        [ActionName("Like")]
        public async Task<IActionResult> CreateLikeAsyn(CreateLikeRequestModel model)
        {
            var like = await _likeService.CreateLike(model);
            if(like.Success == true)
            {
                return Ok(like);
            }
            return StatusCode(400,like);
        }
        [HttpGet("GetLikesByReviewId/{reviewId}")]
        public async Task<IActionResult> GetLikesByReviewIdAsync([FromRoute] int reviewId)
        {
            var likes = await _likeService.GetLikesByReviewIdAsync(reviewId);
            if(likes.Success == true)
            {
                return Ok(likes);
            }
            return StatusCode(400,likes);
        }
    }
}
