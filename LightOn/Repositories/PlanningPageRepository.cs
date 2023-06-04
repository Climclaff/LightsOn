using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}
