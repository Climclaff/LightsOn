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
    public class ApplianceUsagePlannedRepository : IApplianceUsagePlannedRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public ApplianceUsagePlannedRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(ApplianceUsagePlanned usagePlan)
        {
            try
            {
                await _context.ApplianceUsagePlanneds.AddAsync(usagePlan);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding usage plan to database.", ex);
                throw new RepositoryException("Error adding usage plan", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usagePlan = await _context.ApplianceUsagePlanneds.FindAsync(id);
            if (usagePlan == null)
            {
                _logger.LogError($"Error deleting usage plan from database with ID {id}", null);
                throw new NotFoundException($"Usage plan with id {id} not found.");
            }
            _context.ApplianceUsagePlanneds.Remove(usagePlan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ApplianceUsagePlanned>> GetByUserAsync(int id)
        {
            try
            {
                var result = await _context.ApplianceUsagePlanneds.Where(usage => usage.Appliance.UserId == id).ToListAsync();
                if (result == null)
                {
                    throw new NotFoundException($"Usage plan for user with id {id} not found.");
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
        public async Task<ApplianceUsagePlanned> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.ApplianceUsagePlanneds.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Usage plan with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding usage plan with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving usage plan with ID {id}", ex);
                throw new RepositoryException($"Error retrieving usage plan with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(ApplianceUsagePlanned usagePlan)
        {
            try
            {
                var result = await _context.ApplianceUsagePlanneds.AsNoTracking().FirstOrDefaultAsync(current => current.Id == usagePlan.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Usage plan with id {usagePlan.Id} not found.");
                }
                _context.Entry(usagePlan).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating usage plan with ID {usagePlan.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating usage plan with ID {usagePlan.Id}", ex);
                throw new RepositoryException($"Error updating usage plan with ID {usagePlan.Id}", ex);
            }
        }

        public async Task<List<ApplianceUsagePlanned>> GetRangeAsync(int offset, int count)
        {
            var totalPlans = await _context.ApplianceUsagePlanneds.CountAsync();

            if (offset >= totalPlans)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of usage histories {totalPlans}");
            }

            var usagePlans = await _context.ApplianceUsagePlanneds.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalPlans - offset)).ToListAsync();

            return usagePlans;
        }

        public async Task<List<ApplianceUsagePlanned>> GetAllAsync()
        {
            try
            {
                return await _context.ApplianceUsagePlanneds.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all usage plans", ex);
                throw new RepositoryException("Failed to get all usage plans", ex);
            }
        }




    }
}
