using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
#pragma warning disable 8602
namespace LightOn.Repositories
{
    public class PlanningPageRepository : IPlanningPageRepository
    {    
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public PlanningPageRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int?> GetTransformerByUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with id {id} not found.");
            }
            try
            {
                var building = await _context.Buildings.Where(u => u.Id == user.BuildingId).FirstOrDefaultAsync();
                return building.TransformerId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding district to database.", ex);
                throw new RepositoryException("Error adding district", ex);
            }

        }

        public async Task<float> GetTransformerLoad(int id)
        {
            var measurement = await _context.TransformerMeasurements.Where(tr => tr.TransformerId == id).OrderByDescending(m => m.Date)
                .FirstOrDefaultAsync(); 
            if (measurement == null)
            {
                throw new NotFoundException($"Measurement for transformer with id {id} not found.");
            }               
                return measurement.CurrentLoad;            
        }

    }
}
