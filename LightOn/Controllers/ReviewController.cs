using LightOn.Helpers;
using LightOn.Models;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly UserManager<User> _userManager;
        public ReviewController(IReviewService reviewService, UserManager<User> userManager)
        {
            _service = reviewService;
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            bool isAdmin = false;
            bool isOwner = false;
            if (user == null)
            {
                return BadRequest();
            }
            ServiceResponse<Review> review = await _service.GetByIdAsync(id);
            if(review.Data == null)
            {
                return NotFound();
            }
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            if (claims.Count > 0) {
                if (claims.First().Type.ToString() == "IsAdmin" && claims.First().Value.ToString() == "true")
                {
                    isAdmin = true;
                }
            }
            if(review.Data.UserId == user.Id)
            {
                isOwner= true;
            }
            
            if (isAdmin == true || isOwner == true)
            {
                ServiceResponse<Review> result = await _service.DeleteAsync(id);
                if (result.NotFound)
                {
                    return NotFound();
                }
                if (result.Success)
                {
                    return Ok();
                }
                return StatusCode(500, result.ErrorMessage);
            }
            return StatusCode(401, "You don't have access to delete this.");
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            ServiceResponse<Review> result = await _service.CreateAsync(review);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
        [HttpGet]
        [Route("FindById")]
        public async Task<IActionResult> FindById([FromQuery] int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
        [HttpGet]
        [Route("GetRange")]
        public async Task<IActionResult> GetRangeAsync([FromQuery] int offset, int count)
        {
            var result = await _service.GetRangeAsync(offset, count);
            if (result.Success)
            {
                if (result.Data == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            if (result.Success)
            {
                if (result.Data == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }
    }
}
