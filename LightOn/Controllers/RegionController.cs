using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpPost]
        [Route("DeleteRegion")]
        public async Task<IActionResult> DeleteRegion([FromQuery] int id)
        {
            ServiceResponse<Region> result = await _regionService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        [Route("CreateRegion")]
        public async Task<IActionResult> CreateRegion([FromBody] Region region)
        {
            ServiceResponse<Region> result = await _regionService.CreateAsync(region);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        [Route("UpdateRegion")]
        public async Task<IActionResult> UpdateRegion([FromBody] Region region)
        {
            ServiceResponse<Region> result = await _regionService.UpdateAsync(region);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);

        }
        [HttpGet]
        [Route("FindRegionById")]
        public async Task<IActionResult> FindRegionById([FromQuery] int id)
        {
            var response = await _regionService.GetByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("GetRange")]
        public async Task<IActionResult> GetRangeAsync([FromQuery] int offset, int count)
        {
            var response = await _regionService.GetRangeAsync(offset, count);

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("GetAllRegions")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var response = await _regionService.GetAllAsync();

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
    }
}
