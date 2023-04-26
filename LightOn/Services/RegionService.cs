using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly ILoggingService _logger;
        public RegionService(IRegionRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Region>> CreateAsync(Region region)
        {
            try
            {
                await _repository.CreateAsync(region);
                return new ServiceResponse<Region> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Region> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Region>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Region> { Success = false, ErrorMessage = $"Could not delete region with ID {id}" };
                }
                return new ServiceResponse<Region> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Region> { Success = false, NotFound = true, ErrorMessage = ex.Message};
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Region> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Region>> GetByIdAsync(int id)
        {
            try
            {
                var region = await _repository.GetByIdAsync(id);
                if (region == null)
                {
                    return new ServiceResponse<Region> { Success = false, ErrorMessage = $"Region with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<Region> { Success = true, Data = region };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Region> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Region> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Region>> UpdateAsync(Region region)
        {
            try
            {
                await _repository.UpdateAsync(region);
                return new ServiceResponse<Region> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Region> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Region> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Region>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var regions = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Region>> { Success = true, Data = regions };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting regions with offeset {offset}", ex);
                return new ServiceResponse<List<Region>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Region>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Region>> {Success = true, Data = result};
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Region>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
