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

namespace LightOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanningPageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PlanningPageController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

       
        [HttpGet]
        [Route("TestLoad")]
        public async Task<IActionResult> TestLoad()
        {
            TransformerLoadCalculator calculator = new TransformerLoadCalculator();
            var transf = await _context.Transformers.FindAsync(1);
            List<ApplianceUsagePlanned> applianceUsageForTransformer = _context.ApplianceUsagePlanneds
                .Where(u => u.Appliance.User.Building.TransformerId == transf.Id)
                .ToList();
            List<int?> applianceIds = applianceUsageForTransformer.Select(u => u.ApplianceId).ToList();
            List<Appliance> appliancesUsed = await _context.Appliances.Where(a => applianceIds.Contains(a.Id)).ToListAsync();
            var result = calculator.GenerateSegments(transf, applianceUsageForTransformer, appliancesUsed);
            return Ok(result);
        }
    }
}
