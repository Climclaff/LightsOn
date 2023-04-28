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
    public class TownRepository : ITownRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public TownRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Town town)
        {
            try
            {
                await _context.Towns.AddAsync(town);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding town to database.", ex);
                throw new RepositoryException("Error adding town", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var town = await _context.Towns.FindAsync(id);
            if (town == null)
            {
                _logger.LogError($"Error deleting town from database with ID {id}", null);
                throw new NotFoundException($"Town with id {id} not found.");
            }
            _context.Towns.Remove(town);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Town> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Towns.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Town with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding town with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving town with ID {id}", ex);
                throw new RepositoryException($"Error retrieving town with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Town town)
        {
            try
            {
                var result = await _context.Towns.AsNoTracking().FirstOrDefaultAsync(current => current.Id == town.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Town with id {town.Id} not found.");
                }
                _context.Entry(town).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating town with ID {town.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating town with ID {town.Id}", ex);
                throw new RepositoryException($"Error updating town with ID {town.Id}", ex);
            }
        }

        public async Task<List<Town>> GetRangeAsync(int offset, int count)
        {
            var totalTowns = await _context.Towns.CountAsync();

            if (offset >= totalTowns)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of towns {totalTowns}");
            }

            var towns = await _context.Towns.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalTowns - offset)).ToListAsync();

            return towns;
        }

        public async Task<List<Town>> GetAllAsync()
        {
            try
            {
                return await _context.Towns.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all towns", ex);
                throw new RepositoryException("Failed to get all towns", ex);
            }
        }
    }
}
