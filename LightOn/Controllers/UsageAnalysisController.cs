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
        public async Task<IActionResult> HistogramByUserConsumption([FromQuery] int id)
        {
            var result = await _service.HistogramByUserConsumption(id);
            if (result.Success)
            {
                if (result.Data.Item1 == null || result.Data.Item2 == null)
                {
                    return NotFound();
                }
                return Ok(new
                {
                    result.Data.Item1,
                    result.Data.Item2
                });
            }
            return StatusCode(500, result.ErrorMessage);
        }
        [HttpGet]
        [Route("LineChartByUserConsumption")]
        public async Task<IActionResult> LineChartByUserConsumption([FromQuery] int id)
        {
            var result = await _service.LineChartByUserConsumption(id);
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
        public async Task<IActionResult> BarChartByUserConsumption([FromQuery] int id)
        {
            var result = await _service.BarChartByUserConsumption(id);
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
        public async Task<IActionResult> ScatterChartByUserConsumption([FromQuery] int id)
        {
            var result = await _service.ScatterChartByUserConsumption(id);
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
        public async Task<IActionResult> PieChartByUserConsumption([FromQuery] int id)
        {
            var result = await _service.PieChartByUserConsumption(id);
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
