using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Migrations;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using LightOn.BLL;
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
                 await RemoveUserExpiredPlansAsync(id);
                 var result = await _context.ApplianceUsagePlanneds
                    .Where(usage => usage.Appliance.UserId == id)
                    .ToListAsync();
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
        public async Task<List<ApplianceUsagePlanned>> GetApplianceUsageForTransformer(int id)
        {
            try
            {
                var result = await _context.Transformers.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Transformer with id {id} not found.");
                }
                await RemoveAllExpiredPlansAsync();
                return await _context.ApplianceUsagePlanneds
                    .Where(u => u.Appliance.User.Building.TransformerId == id)
                    .ToListAsync();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding appliance usage for transformer with with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving appliance usage for transformer with with ID {id}", ex);
                throw new RepositoryException($"Error retrieving appliance usage for transformer with ID {id}", ex);
            }
        }
        public async Task<ApplianceUsagePlanned> GetByIdAsync(int id)
        {
            try
            {
                await RemoveExpiredPlanByIdAsync(id);
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
            await RemoveAllExpiredPlansAsync();
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
                await RemoveAllExpiredPlansAsync();
                return await _context.ApplianceUsagePlanneds.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all usage plans", ex);
                throw new RepositoryException("Failed to get all usage plans", ex);
            }
        }
        private async Task RemoveUserExpiredPlansAsync(int id)
        {
            var currentTime = DateTime.Now;

            var expiredUsagePlanned = await _context.ApplianceUsagePlanneds
                .Where(usage => usage.Appliance.UserId == id && usage.UsageEndDate < currentTime)
                .ToListAsync();

            if (expiredUsagePlanned.Any())
            {
                foreach (var plan in expiredUsagePlanned)
                {
                    var appliance = await _context.Appliances.Where(appliance => appliance.Id == plan.ApplianceId).FirstOrDefaultAsync();
                    var energy = ConsumptionCalculator.Calculate(appliance.Power, plan.UsageStartDate, plan.UsageEndDate);
                    var usageHistory = new ApplianceUsageHistory
                    {
                        UsageStartDate = plan.UsageStartDate,
                        UsageEndDate = plan.UsageEndDate,
                        ApplianceId = plan.ApplianceId,
                        EnergyConsumed = energy
                    };

                    await _context.ApplianceUsageHistories.AddAsync(usageHistory);
                    _context.ApplianceUsagePlanneds.Remove(plan);
                }
                await _context.SaveChangesAsync();
            }
        }
        private async Task RemoveAllExpiredPlansAsync()
        {
            var currentTime = DateTime.Now;

            var expiredUsagePlanned = await _context.ApplianceUsagePlanneds
                .Where(usage => usage.UsageEndDate < currentTime)
                .ToListAsync();

            if (expiredUsagePlanned.Any())
            {              
                foreach (var plan in expiredUsagePlanned)
                {
                    var appliance = await _context.Appliances.Where(appliance => appliance.Id == plan.ApplianceId).FirstOrDefaultAsync();
                    var energy = ConsumptionCalculator.Calculate(appliance.Power, plan.UsageStartDate, plan.UsageEndDate);
                    var usageHistory = new ApplianceUsageHistory
                    {
                        UsageStartDate = plan.UsageStartDate,
                        UsageEndDate = plan.UsageEndDate,
                        ApplianceId = plan.ApplianceId,
                        EnergyConsumed = energy
                    };

                    await _context.ApplianceUsageHistories.AddAsync(usageHistory);
                    _context.ApplianceUsagePlanneds.Remove(plan);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task RemoveExpiredPlanByIdAsync(int id)
        {
            var currentTime = DateTime.Now;

            var expiredUsagePlan = await _context.ApplianceUsagePlanneds
                .Where(plan => plan.Id == id && plan.UsageEndDate < currentTime)
                .SingleOrDefaultAsync();

            if (expiredUsagePlan != null)
            {
                var appliance = await _context.Appliances.Where(appliance => appliance.Id == expiredUsagePlan.ApplianceId).FirstOrDefaultAsync();
                var energy = ConsumptionCalculator.Calculate(appliance.Power, expiredUsagePlan.UsageStartDate, expiredUsagePlan.UsageEndDate);
                var usageHistory = new ApplianceUsageHistory
                {
                    UsageStartDate = expiredUsagePlan.UsageStartDate,
                    UsageEndDate = expiredUsagePlan.UsageEndDate,
                    ApplianceId = expiredUsagePlan.ApplianceId,
                    EnergyConsumed = energy
                };

                    await _context.ApplianceUsageHistories.AddAsync(usageHistory);
                    _context.ApplianceUsagePlanneds.Remove(expiredUsagePlan);
                
                await _context.SaveChangesAsync();
            }
        }



    }
}
