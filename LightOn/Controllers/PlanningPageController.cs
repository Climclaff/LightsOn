using LightOn.Models.ClientModels;
using LightOn.Models;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LightOn.BLL;
using LightOn.Data;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Text;
using System.Collections.Concurrent;
using System.Text.Json;
#pragma warning disable 8600
namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanningPageController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly UserManager<User> _userManager;
        private readonly IPlanningPageService _service;
        public PlanningPageController(IDistributedCache cache, IPlanningPageService service, UserManager<User> userManager)
        {
            _cache = cache;
            _userManager = userManager;
            _service = service;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetLoadInfo")]
        public async Task<IActionResult> GetLoadInfo()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _service.GetTransformerByUserAsync(user.Id);
            if (result.Data== null)
            {
                return BadRequest();
            }
                var dictionaryByteArray = await _cache.GetAsync(result.Data.ToString() + "Planning");
                if (dictionaryByteArray != null)
                {

                    string json = Encoding.UTF8.GetString(dictionaryByteArray);
                    ConcurrentDictionary<DateTime, float> dictionary = JsonSerializer.Deserialize<ConcurrentDictionary<DateTime, float>>(json);
                    var currentLoad = await _service.GetTransformerLoad((int)result.Data);
                    return Ok(new
                    {
                        dict = dictionary,
                        currTransfLoad = currentLoad
                    });
                }                      
            else
            {
                return StatusCode(500, "No transformer information available.");
            }
        }
    }
}
