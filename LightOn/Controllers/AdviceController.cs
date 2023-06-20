using LightOn.Models;
using LightOn.Services;
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
    public class AdviceController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAdviceService _adviceService;
        public AdviceController(UserManager<User> userManager, IAdviceService adviceService)
        {
            _userManager = userManager;
            _adviceService = adviceService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsPremiumPolicy")]
        [HttpGet]
        [Route("GenerateTips")]
        public async Task<IActionResult> GenerateTips()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _adviceService.GenerateTipsAsync(user.Id);
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
    }
}
