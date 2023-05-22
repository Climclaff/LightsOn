using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class ApplianceUsagePlannedService : IApplianceUsagePlannedService
    {
        private readonly IApplianceUsagePlannedRepository _repository;
        private readonly ILoggingService _logger;
        public ApplianceUsagePlannedService(IApplianceUsagePlannedRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<ApplianceUsagePlanned>> CreateAsync(ApplianceUsagePlanned usagePlan)
        {
            try
            {
                await _repository.CreateAsync(usagePlan);
                return new ServiceResponse<ApplianceUsagePlanned> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<ApplianceUsagePlanned>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = $"Could not delete usage plan with ID {id}" };
                }
                return new ServiceResponse<ApplianceUsagePlanned> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ServiceResponse<List<ApplianceUsagePlanned>>> GetByUserAsync(int id)
        {
            try
            {

                var usagePlans = await _repository.GetByUserAsync(id);
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = true, Data = usagePlans };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting usage plan for user with id {id}", ex);
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<ApplianceUsagePlanned>> GetByIdAsync(int id)
        {
            try
            {
                var usagePlan = await _repository.GetByIdAsync(id);
                if (usagePlan == null)
                {
                    return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = $"Usage plan with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<ApplianceUsagePlanned> { Success = true, Data = usagePlan };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<ApplianceUsagePlanned>> UpdateAsync(ApplianceUsagePlanned usagePlan)
        {
            try
            {
                await _repository.UpdateAsync(usagePlan);
                return new ServiceResponse<ApplianceUsagePlanned> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsagePlanned> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<ApplianceUsagePlanned>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var usagePlans = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = true, Data = usagePlans };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting usage plans with offeset {offset}", ex);
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<ApplianceUsagePlanned>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<ApplianceUsagePlanned>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
