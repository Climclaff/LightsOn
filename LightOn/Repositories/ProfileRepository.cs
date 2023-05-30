using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Models.ClientModels;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8602
namespace LightOn.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public ProfileRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> ChangeLocationAsync(int userId, int regionId, int districtId, int townId, int streetId, int buildingId)
        {
            try
            {
                var result = await _context.Users.FirstOrDefaultAsync(current => current.Id == userId);
                if (result == null)
                {
                    throw new NotFoundException($"User with id {userId} not found.");
                }
                result.RegionId = regionId;
                result.DistrictId = districtId;
                result.TownId = townId;
                result.StreetId = streetId;
                result.BuildingId = buildingId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating location for user with ID {userId}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating location for user with ID {userId}", ex);
                throw new RepositoryException($"Error updating location for user with ID {userId}", ex);
            }
        }


        public async Task<List<Region>> GetRegionsAsync()
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
        public async Task<List<District>> GetDistrictsOfRegionAsync(int id)
        {
            try
            {
                var result = await _context.Regions.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Region with id {id} not found.");
                }
                return await _context.Districts.Where(u => u.RegionId == id).ToListAsync();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding region with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving districts of region with ID {id}", ex);
                throw new RepositoryException($"Error retrieving districts of region with ID {id}", ex);
            }
            
        }
        public async Task<List<Town>> GetTownsOfDistrictAsync(int id)
        {
            try
            {
                var result = await _context.Districts.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"District with id {id} not found.");
                }
                return await _context.Towns.Where(u => u.DistrictId == id).ToListAsync();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding district with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving towns of district with ID {id}", ex);
                throw new RepositoryException($"Error retrieving towns of district with ID {id}", ex);
            }
        }
        public async Task<List<Street>> GetStreetsOfTownAsync(int id)
        {
            try
            {
                var result = await _context.Towns.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Town with id {id} not found.");
                }
                return await _context.Streets.Where(u => u.TownId == id).ToListAsync();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding town with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving streets of town with ID {id}", ex);
                throw new RepositoryException($"Error retrieving streets of town with ID {id}", ex);
            }
        }
        public async Task<List<Building>> GetBuildingsOfStreetAsync(int id)
        {
            try
            {
                var result = await _context.Streets.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Street with id {id} not found.");
                }
                return await _context.Buildings.Where(u => u.StreetId == id).ToListAsync();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding street with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving buildings of street with ID {id}", ex);
                throw new RepositoryException($"Error retrieving buildings of street with ID {id}", ex);
            }
        }

        public async Task<bool> ChangeImageAsync(int userId, byte[] imgData)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with id {userId} not found.");
                }
                user.ImageData = imgData;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload user image", ex);
                throw new RepositoryException("Failed to upload user image", ex);
            }
        }

        public async Task<bool> ChangeNameAsync(int userId, ChangeNameModel model)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with id {userId} not found.");
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to change user name", ex);
                throw new RepositoryException("Failed to change user name", ex);
            }
        }
        public async Task<bool> ChangeBuildingAreaAsync(int userId, int area)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with id {userId} not found.");
                }
                var building = await _context.Buildings.FindAsync(user.BuildingId);
                building.Area = area;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to change building area", ex);
                throw new RepositoryException("Failed to change building area", ex);
            }
        }

    }
}
