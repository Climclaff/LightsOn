using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;
        private readonly ILoggingService _logger;
        public BuildingService(IBuildingRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Building>> CreateAsync(Building building)
        {
            try
            {
                await _repository.CreateAsync(building);
                return new ServiceResponse<Building> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Building> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Building>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Building> { Success = false, ErrorMessage = $"Could not delete building with ID {id}" };
                }
                return new ServiceResponse<Building> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Building> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Building>> GetByIdAsync(int id)
        {
            try
            {
                var building = await _repository.GetByIdAsync(id);
                if (building == null)
                {
                    return new ServiceResponse<Building> { Success = false, ErrorMessage = $"Building with ID {id} was not found" };
                }
                return new ServiceResponse<Building> { Success = true, Data = building };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Building> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Building>> UpdateAsync(Building building)
        {
            try
            {
                await _repository.UpdateAsync(building);
                return new ServiceResponse<Building> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Building> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Building>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var buildings = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Building>> { Success = true, Data = buildings };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting buildings with offeset {offset}", ex);
                return new ServiceResponse<List<Building>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Building>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Building>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Building>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
