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
    public class StreetRepository : IStreetRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public StreetRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Street street)
        {
            try
            {
                await _context.Streets.AddAsync(street);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding street to database.", ex);
                throw new RepositoryException("Error adding street", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var street = await _context.Streets.FindAsync(id);
            if (street == null)
            {
                _logger.LogError($"Error deleting street from database with ID {id}", null);
                throw new NotFoundException($"Street with id {id} not found.");
            }
            _context.Streets.Remove(street);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Street> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Streets.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Street with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding street with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving street with ID {id}", ex);
                throw new RepositoryException($"Error retrieving street with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Street street)
        {
            try
            {
                var result = await _context.Streets.AsNoTracking().FirstOrDefaultAsync(current => current.Id == street.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Street with id {street.Id} not found.");
                }
                _context.Entry(street).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating street with ID {street.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating street with ID {street.Id}", ex);
                throw new RepositoryException($"Error updating street with ID {street.Id}", ex);
            }
        }

        public async Task<List<Street>> GetRangeAsync(int offset, int count)
        {
            var totalStreets = await _context.Streets.CountAsync();

            if (offset >= totalStreets)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of streets {totalStreets}");
            }

            var streets = await _context.Streets.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalStreets - offset)).ToListAsync();

            return streets;
        }

        public async Task<List<Street>> GetAllAsync()
        {
            try
            {
                return await _context.Streets.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all streets", ex);
                throw new RepositoryException("Failed to get all streets", ex);
            }
        }
    }
}
