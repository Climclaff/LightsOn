﻿using LightOn.Helpers;
using LightOn.Models;
using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ITownService _service;

        public TownController(ITownService townService)
        {
            _service = townService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            ServiceResponse<Town> result = await _service.DeleteAsync(id);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Town town)
        {
            ServiceResponse<Town> result = await _service.CreateAsync(town);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Town town)
        {
            ServiceResponse<Town> result = await _service.UpdateAsync(town);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminPolicy")]
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
