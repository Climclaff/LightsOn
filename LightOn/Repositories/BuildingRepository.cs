using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8602 
#pragma warning disable CS8603 
namespace LightOn.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public BuildingRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Building building)
        {
            try
            {
                await _context.Buildings.AddAsync(building);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding building to database.", ex);
                throw new RepositoryException("Error adding building", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                _logger.LogError($"Error deleting building from database with ID {id}", null);
                throw new NotFoundException($"Building with id {id} not found.");
            }
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Building> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Buildings.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Building with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding building with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving building with ID {id}", ex);
                throw new RepositoryException($"Error retrieving building with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Building building)
        {
            try
            {
                var result = await _context.Buildings.AsNoTracking().FirstOrDefaultAsync(current => current.Id == building.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Building with id {building.Id} not found.");
                }
                _context.Entry(building).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating building with ID {building.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating building with ID {building.Id}", ex);
                throw new RepositoryException($"Error updating building with ID {building.Id}", ex);
            }
        }

        public async Task<List<Building>> GetRangeAsync(int offset, int count)
        {
            var totalBuildings = await _context.Buildings.CountAsync();

            if (offset >= totalBuildings)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of buildings {totalBuildings}");
            }

            var buildings = await _context.Buildings.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalBuildings - offset)).ToListAsync();

            return buildings;
        }

        public async Task<List<Building>> GetAllAsync()
        {
            try
            {
                return await _context.Buildings.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all buildings", ex);
                throw new RepositoryException("Failed to get all buildings", ex);
            }
        }
    }
}
