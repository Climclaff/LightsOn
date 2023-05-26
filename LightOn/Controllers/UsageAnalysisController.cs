using LightOn.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsageAnalysisController : ControllerBase
    {
        private readonly IApplianceUsageService _service;

        public UsageAnalysisController(IApplianceUsageService applianceUsageService)
        {
            _service = applianceUsageService;
        }

        [HttpGet]
        [Route("HistogramByUserConsumption")]
        public async Task<IActionResult> HistogramByUserConsumption([FromQuery] int id, DateTime startDate)
        {
            var result = await _service.HistogramByUserConsumption(id, startDate);
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
        [HttpGet]
        [Route("LineChartByUserConsumption")]
        public async Task<IActionResult> LineChartByUserConsumption([FromQuery] int id, DateTime startDate)
        {
            var result = await _service.LineChartByUserConsumption(id, startDate);
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
        [HttpGet]
        [Route("BarChartByUserConsumption")]
        public async Task<IActionResult> BarChartByUserConsumption([FromQuery] int id, DateTime startDate)
        {
            var result = await _service.BarChartByUserConsumption(id, startDate);
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
        [HttpGet]
        [Route("ScatterChartByUserConsumption")]
        public async Task<IActionResult> ScatterChartByUserConsumption([FromQuery] int id, DateTime startDate)
        {
            var result = await _service.ScatterChartByUserConsumption(id, startDate);
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
        [HttpGet]
        [Route("PieChartByUserConsumption")]
        public async Task<IActionResult> PieChartByUserConsumption([FromQuery] int id, DateTime startDate)
        {
            var result = await _service.PieChartByUserConsumption(id, startDate);
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
