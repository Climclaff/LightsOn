using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class ApplianceService : IApplianceService
    {
        private readonly IApplianceRepository _repository;
        private readonly ILoggingService _logger;
        public ApplianceService(IApplianceRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Appliance>> CreateAsync(Appliance appliance)
        {
            try
            {
                await _repository.CreateAsync(appliance);
                return new ServiceResponse<Appliance> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Appliance> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Appliance>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Appliance> { Success = false, ErrorMessage = $"Could not delete appliance with ID {id}" };
                }
                return new ServiceResponse<Appliance> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Appliance> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Appliance>> GetByIdAsync(int id)
        {
            try
            {
                var appliance = await _repository.GetByIdAsync(id);
                if (appliance == null)
                {
                    return new ServiceResponse<Appliance> { Success = false, ErrorMessage = $"Appliance with ID {id} was not found" };
                }
                return new ServiceResponse<Appliance> { Success = true, Data = appliance };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Appliance> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Appliance>> UpdateAsync(Appliance appliance)
        {
            try
            {
                await _repository.UpdateAsync(appliance);
                return new ServiceResponse<Appliance> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Appliance> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Appliance>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var appliances = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Appliance>> { Success = true, Data = appliances };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting appliances with offeset {offset}", ex);
                return new ServiceResponse<List<Appliance>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Appliance>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Appliance>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Appliance>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
