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
    public class DistrictRepository : IDistrictRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public DistrictRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(District district)
        {
            try
            {
                await _context.Districts.AddAsync(district);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding district to database.", ex);
                throw new RepositoryException("Error adding district", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                _logger.LogError($"Error deleting district from database with ID {id}", null);
                return false;
            }
            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<District> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Districts.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving district with ID {id}", ex);
                throw new RepositoryException($"Error retrieving district with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(District district)
        {
            try
            {
                _context.Entry(district).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating district with ID {district.Id}", ex);
                throw new RepositoryException($"Error updating district with ID {district.Id}", ex);
            }
        }

        public async Task<List<District>> GetRangeAsync(int offset, int count)
        {
            var totalDistricts = await _context.Districts.CountAsync();

            if (offset >= totalDistricts)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of districts {totalDistricts}");
            }

            var districts = await _context.Districts.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalDistricts - offset)).ToListAsync();

            return districts;
        }

        public async Task<List<District>> GetAllAsync()
        {
            try
            {
                return await _context.Districts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all districts", ex);
                throw new RepositoryException("Failed to get all districts", ex);
            }
        }
    }
}
