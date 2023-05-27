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
    public class UsageAnalysisController : ControllerBase
    {
        private readonly IApplianceUsageService _service;
        private readonly UserManager<User> _userManager;
        public UsageAnalysisController(UserManager<User> userManager, IApplianceUsageService applianceUsageService)
        {
            _userManager = userManager;
            _service = applianceUsageService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("HistogramByUserConsumption")]
        public async Task<IActionResult> HistogramByUserConsumption(DateTime startDate)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.HistogramByUserConsumption(user.Id, startDate);
            if (result.Success)
            {
                if (result.Data == null)
                {
                    return NotFound();
                }
                return Ok(new
                {
                    result.Data
                });
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("LineChartByUserConsumption")]
        public async Task<IActionResult> LineChartByUserConsumption(DateTime startDate)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.LineChartByUserConsumption(user.Id, startDate);
            if (result.Success)
            {
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("BarChartByUserConsumption")]
        public async Task<IActionResult> BarChartByUserConsumption(DateTime startDate)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.BarChartByUserConsumption(user.Id, startDate);
            if (result.Success)
            {
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("ScatterChartByUserConsumption")]
        public async Task<IActionResult> ScatterChartByUserConsumption(DateTime startDate)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.ScatterChartByUserConsumption(user.Id, startDate);
            if (result.Success)
            {
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("PieChartByUserConsumption")]
        public async Task<IActionResult> PieChartByUserConsumption(DateTime startDate)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.PieChartByUserConsumption(user.Id, startDate);
            if (result.Success)
            {
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

    }
}
