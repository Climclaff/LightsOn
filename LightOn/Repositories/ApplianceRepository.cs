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
    public class ApplianceRepository : IApplianceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public ApplianceRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Appliance appliance)
        {
            try
            {
                await _context.Appliances.AddAsync(appliance);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding appliance to database.", ex);
                throw new RepositoryException("Error adding appliance", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appliance = await _context.Appliances.FindAsync(id);
            if (appliance == null)
            {
                _logger.LogError($"Error deleting appliance from database with ID {id}", null);
                throw new NotFoundException($"Appliance with id {id} not found.");
            }
            _context.Appliances.Remove(appliance);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Appliance>> GetUserAppliancesAsync(int id)
        {
            try
            {
                return await _context.Appliances.Where(a => a.UserId == id).Select(a => new Appliance
                {
                    Id = a.Id,
                    Name = a.Name,
                    Power= a.Power,
                    PowerFactor= a.PowerFactor,
                    Amperage= a.Amperage,
                    UserId= a.UserId,
                    imageIndex = a.imageIndex
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error finding appliances in database.", ex);
                throw new RepositoryException("Error finding user's appliances", ex);
            }
        }
        public async Task<Appliance> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Appliances.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Appliance with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding appliance with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving appliance with ID {id}", ex);
                throw new RepositoryException($"Error retrieving appliance with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Appliance appliance)
        {
            try
            {
                var result = await _context.Appliances.AsNoTracking().FirstOrDefaultAsync(current => current.Id == appliance.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Appliance with id {appliance.Id} not found.");
                }
                _context.Entry(appliance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating appliance with ID {appliance.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating appliance with ID {appliance.Id}", ex);
                throw new RepositoryException($"Error updating appliance with ID {appliance.Id}", ex);
            }
        }

        public async Task<List<Appliance>> GetRangeAsync(int offset, int count)
        {
            var totalAppliances = await _context.Appliances.CountAsync();

            if (offset >= totalAppliances)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of appliances {totalAppliances}");
            }

            var appliances = await _context.Appliances.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalAppliances - offset)).ToListAsync();

            return appliances;
        }

        public async Task<List<Appliance>> GetAllAsync()
        {
            try
            {
                return await _context.Appliances.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all appliances", ex);
                throw new RepositoryException("Failed to get all appliances", ex);
            }
        }
    }
}
