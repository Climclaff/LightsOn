﻿using LightOn.Helpers;
using LightOn.Models;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplianceUsagePlannedController : ControllerBase
    {
        private readonly IApplianceUsagePlannedService _service;

        public ApplianceUsagePlannedController(IApplianceUsagePlannedService applianceUsagePlannedService)
        {
            _service = applianceUsagePlannedService;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            ServiceResponse<ApplianceUsagePlanned> result = await _service.DeleteAsync(id);
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
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ApplianceUsagePlanned usagePlan)
        {
            ServiceResponse<ApplianceUsagePlanned> result = await _service.CreateAsync(usagePlan);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ApplianceUsagePlanned usagePlan)
        {
            ServiceResponse<ApplianceUsagePlanned> result = await _service.UpdateAsync(usagePlan);
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

        [HttpGet]
        [Route("GetByUser")]
        public async Task<IActionResult> GetByUser([FromQuery] int id)
        {
            var result = await _service.GetByUserAsync(id);
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