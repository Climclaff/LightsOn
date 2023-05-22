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
    public class ApplianceUsageRepository : IApplianceUsageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public ApplianceUsageRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(ApplianceUsageHistory usageHistory)
        {
            try
            {
                await _context.ApplianceUsageHistories.AddAsync(usageHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding usage history to database.", ex);
                throw new RepositoryException("Error adding usage history", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usageHistory = await _context.ApplianceUsageHistories.FindAsync(id);
            if (usageHistory == null)
            {
                _logger.LogError($"Error deleting usage history from database with ID {id}", null);
                throw new NotFoundException($"Usage history with id {id} not found.");
            }
            _context.ApplianceUsageHistories.Remove(usageHistory);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<ApplianceUsageHistory> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.ApplianceUsageHistories.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Usage history with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding usage history with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving usage history with ID {id}", ex);
                throw new RepositoryException($"Error retrieving usage history with ID {id}", ex);
            }
        }

        public async Task<List<ApplianceUsageHistory>> GetByUserAsync(int id)
        {
            try
            {
                var result = await _context.ApplianceUsageHistories
                   .Where(usage => usage.Appliance.UserId == id)
                   .ToListAsync();
                if (result == null)
                {
                    throw new NotFoundException($"Usage history for user with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding usage plan for user with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving usage plan for user with ID {id}", ex);
                throw new RepositoryException($"Error retrieving usage plan for user with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(ApplianceUsageHistory usageHistory)
        {
            try
            {
                var result = await _context.ApplianceUsageHistories.AsNoTracking().FirstOrDefaultAsync(current => current.Id == usageHistory.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Usage history with id {usageHistory.Id} not found.");
                }
                _context.Entry(usageHistory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating usage history with ID {usageHistory.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating usage history with ID {usageHistory.Id}", ex);
                throw new RepositoryException($"Error updating usage history with ID {usageHistory.Id}", ex);
            }
        }

        public async Task<List<ApplianceUsageHistory>> GetRangeAsync(int offset, int count)
        {
            var totalHistories = await _context.ApplianceUsageHistories.CountAsync();

            if (offset >= totalHistories)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of usage histories {totalHistories}");
            }

            var usageHistories = await _context.ApplianceUsageHistories.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalHistories - offset)).ToListAsync();

            return usageHistories;
        }

        public async Task<List<ApplianceUsageHistory>> GetAllAsync()
        {
            try
            {
                return await _context.ApplianceUsageHistories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all usage histories", ex);
                throw new RepositoryException("Failed to get all usage histories", ex);
            }
        }
    }
}
