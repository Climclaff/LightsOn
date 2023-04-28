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
    public class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public RegionRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Region region)
        {
            try
            {              
                await _context.Regions.AddAsync(region);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding region to database.", ex);
                throw new RepositoryException("Error adding region", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                _logger.LogError($"Error deleting region from database with ID {id}", null);
                throw new NotFoundException($"Region with id {id} not found.");
            }
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();   
            return true;           
        }
        

        public async Task<Region> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Regions.FindAsync(id);
                if(result == null)
                {
                    throw new NotFoundException($"Region with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding region with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving region with ID {id}", ex);
                throw new RepositoryException($"Error retrieving region with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Region region)
        {
            try
            {
                var result = await _context.Regions.AsNoTracking().FirstOrDefaultAsync(current => current.Id == region.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Region with id {region.Id} not found.");
                }
                _context.Entry(region).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating region with ID {region.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating region with ID {region.Id}", ex);
                throw new RepositoryException($"Error updating region with ID {region.Id}", ex);
            }
        }

        public async Task<List<Region>> GetRangeAsync(int offset, int count)
        {
            var totalRegions = await _context.Regions.CountAsync();

            if (offset >= totalRegions)
            { 
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of regions {totalRegions}");
            }

            var regions = await _context.Regions.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalRegions - offset)).ToListAsync();
            return regions;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            try
            {
                return await _context.Regions.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all regions", ex);
                throw new RepositoryException("Failed to get all regions", ex);
            }
        }
    }
}
