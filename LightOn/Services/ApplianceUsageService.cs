using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class ApplianceUsageService : IApplianceUsageService
    {
        private readonly IApplianceUsageRepository _repository;
        private readonly ILoggingService _logger;
        public ApplianceUsageService(IApplianceUsageRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<ApplianceUsageHistory>> CreateAsync(ApplianceUsageHistory usageHistory)
        {
            try
            {
                await _repository.CreateAsync(usageHistory);
                return new ServiceResponse<ApplianceUsageHistory> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<ApplianceUsageHistory>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = $"Could not delete usage history with ID {id}" };
                }
                return new ServiceResponse<ApplianceUsageHistory> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<ApplianceUsageHistory>> GetByIdAsync(int id)
        {
            try
            {
                var usageHistory = await _repository.GetByIdAsync(id);
                if (usageHistory == null)
                {
                    return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = $"Usage history with ID {id} was not found" };
                }
                return new ServiceResponse<ApplianceUsageHistory> { Success = true, Data = usageHistory };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<ApplianceUsageHistory>> UpdateAsync(ApplianceUsageHistory usageHistory)
        {
            try
            {
                await _repository.UpdateAsync(usageHistory);
                return new ServiceResponse<ApplianceUsageHistory> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<ApplianceUsageHistory>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var usageHistories = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = true, Data = usageHistories };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting usage histories with offeset {offset}", ex);
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<ApplianceUsageHistory>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
