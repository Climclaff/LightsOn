using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _repository;
        private readonly ILoggingService _logger;
        public DistrictService(IDistrictRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<District>> CreateAsync(District district)
        {
            try
            {
                await _repository.CreateAsync(district);
                return new ServiceResponse<District> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<District> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<District>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<District> { Success = false, ErrorMessage = $"Could not delete district with ID {id}" };
                }
                return new ServiceResponse<District> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<District> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<District>> GetByIdAsync(int id)
        {
            try
            {
                var district = await _repository.GetByIdAsync(id);
                if (district == null)
                {
                    return new ServiceResponse<District> { Success = false, ErrorMessage = $"District with ID {id} was not found" };
                }
                return new ServiceResponse<District> { Success = true, Data = district };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<District> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<District>> UpdateAsync(District district)
        {
            try
            {
                await _repository.UpdateAsync(district);
                return new ServiceResponse<District> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<District> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<District>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var districts = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<District>> { Success = true, Data = districts };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting districts with offeset {offset}", ex);
                return new ServiceResponse<List<District>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<District>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<District>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<District>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
