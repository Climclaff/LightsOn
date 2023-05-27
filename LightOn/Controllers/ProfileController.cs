using LightOn.Helpers;
using LightOn.Migrations;
using LightOn.Models;
using LightOn.Models.ClientModels;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
# pragma warning disable CS8601
namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IProfileService _profileService;
        public ProfileController(UserManager<User> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("ProfileInfo")]
        public async Task<IActionResult> ProfileInfo()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            ProfileModel model = new ProfileModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.ImgData = user.ImageData;
            model.regionId = user.RegionId;
            model.districtId = user.DistrictId;
            model.townId = user.TownId;
            model.streetId = user.StreetId;
            model.buildingId = user.BuildingId;
            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetRegions")]
        public async Task<IActionResult> GetRegions()
        {
            var result = await _profileService.GetRegionsAsync();

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetDistrictsOfRegion")]
        public async Task<IActionResult> GetDistrictsOfRegion([FromQuery] int id)
        {
            var result = await _profileService.GetDistrictsOfRegionAsync(id);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetTownsOfDistrict")]
        public async Task<IActionResult> GetTownsOfDistrict([FromQuery] int id)
        {
            var result = await _profileService.GetTownsOfDistrictAsync(id);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetStreetsOfTown")]
        public async Task<IActionResult> GetStreetsOfTown([FromQuery] int id)
        {
            var result = await _profileService.GetStreetsOfTownAsync(id);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetBuildingsOfStreet")]
        public async Task<IActionResult> GetBuildingsOfStreet([FromQuery] int id)
        {
            var result = await _profileService.GetBuildingsOfStreetAsync(id);

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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangeLocation")]
        public async Task<IActionResult> ChangeLocation([FromQuery] int regionId, int districtId, int townId, int streetId, int buildingId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            ServiceResponse<bool> result = await _profileService.ChangeLocation(user.Id, regionId, districtId, townId, streetId, buildingId);
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest();
            }

            return Ok();
        }
        [HttpPost]
        [Route("ChangeImage")]
        public async Task<IActionResult> ChangeImage([FromQuery] int userId, byte[] imgData)
        {
            var result = await _profileService.ChangeImageAsync(userId, imgData);
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangeName")]
        public async Task<IActionResult> ChangeName([FromBody] ChangeNameModel model)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(
                new ValidationProblemDetails(ModelState));
            }
            var result = await _profileService.ChangeNameAsync(user.Id, model);
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
    }
}
