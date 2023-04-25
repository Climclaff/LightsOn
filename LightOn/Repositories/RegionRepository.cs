using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Services;
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
                return false;
            }
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();   
            return true;           
        }
        

        public async Task<Region> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Regions.FindAsync(id);
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
                _context.Entry(region).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating region with ID {region.Id}", ex);
                throw new RepositoryException($"Error updating region with ID {region.Id}", ex);
            }
        }

        public async Task<List<Region>> GetRegionsRangeAsync(int offset, int count)
        {
            var totalRegions = await _context.Regions.CountAsync();

            if (offset >= totalRegions)
            { 
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of regions {totalRegions}");
            }

            var regions = await _context.Regions.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalRegions - offset)).ToListAsync();

            return regions;
        }
    }
}
