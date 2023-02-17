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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }
        [HttpPost("CreateReview/{userId}")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateReviewRequestModel model,[FromRoute]int userId)
        {
            var review = await _reviewServices.CreateRiview(model,userId);
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
        [HttpGet("GetAllByCustomer/{id}")]
        public async Task<IActionResult> GetByCustomer([FromRoute]int id)
        {
            var review = await _reviewServices.GetAllReviewsByCustomerAsync(id);
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            var review = await _reviewServices.GetReviewByIdAsync(id);
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
        [HttpGet("GetAll")]
         public async Task<IActionResult> GetAllAsync()
        {
            var reviews = await _reviewServices.GetAllReviewsAsync();
            if (reviews.Success == false)
            {
                return BadRequest(reviews);
            }
            return Ok(reviews);
        }
        [HttpGet("GetAllUnseenReview")]
         public async Task<IActionResult> GetAllUnseenReviewAsync()
        {
            var reviews = await _reviewServices.GetAllUnSeenReviewsAsync();
            if (reviews.Success == false)
            {
                return BadRequest(reviews);
            }
            return Ok(reviews);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id)
        {
            var review = await _reviewServices.UpdateReviewStatusAsync(id);
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
        [HttpPut("UpdateAll")]
        public async Task<IActionResult> UpdateAllAsync()
        {
            var review = await _reviewServices.UpdateAll();
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            var review = await _reviewServices.DeleteReviewAsync(id);
            if (review.Success == false)
            {
                return BadRequest(review);
            }
            return Ok(review);
        }
    }
}