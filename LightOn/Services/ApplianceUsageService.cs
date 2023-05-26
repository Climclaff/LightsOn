using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using LightOn.BLL;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LightOn.Services
{
    public class ApplianceUsageService : IApplianceUsageService
    {
        private readonly IApplianceUsageRepository _repository;
        private readonly IApplianceRepository _applianceRepository;
        private readonly ILoggingService _logger;
        public ApplianceUsageService(IApplianceUsageRepository repository, IApplianceRepository applianceRepository, ILoggingService logger)
        {
            _applianceRepository = applianceRepository;
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
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ServiceResponse<List<ApplianceUsageHistory>>> GetByUserAsync(int id)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = true, Data = usagePlans };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting usage history for user with id {id}", ex);
                return new ServiceResponse<List<ApplianceUsageHistory>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<ApplianceUsageHistory>> GetByIdAsync(int id)
        {
            try
            {
                var usageHistory = await _repository.GetByIdAsync(id);
                if (usageHistory == null)
                {
                    return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = $"Usage history with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<ApplianceUsageHistory> { Success = true, Data = usageHistory };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, ErrorMessage = ex.Message, NotFound = true };
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
            catch (NotFoundException ex)
            {
                return new ServiceResponse<ApplianceUsageHistory> { Success = false, NotFound = true, ErrorMessage = ex.Message };
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
        public async Task<ServiceResponse<Dictionary<string, object>>> HistogramByUserConsumption(int id, DateTime startDate)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                usagePlans = usagePlans.Where(plan => plan.UsageStartDate >= startDate).ToList();
                var result = EnergyConsumptionAnalyzer.GenerateChart(usagePlans, null, EnergyConsumptionAnalyzer.ChartType.Histogram);
                return new ServiceResponse<Dictionary<string, object>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting histogram for user with id {id}", ex);
                return new ServiceResponse<Dictionary<string, object>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<Dictionary<string, object>>> LineChartByUserConsumption(int id, DateTime startDate)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                usagePlans = usagePlans.Where(plan => plan.UsageStartDate >= startDate).ToList();
                var result = EnergyConsumptionAnalyzer.GenerateChart(usagePlans, null, EnergyConsumptionAnalyzer.ChartType.Line);
                return new ServiceResponse<Dictionary<string, object>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting line chart for user with id {id}", ex);
                return new ServiceResponse<Dictionary<string, object>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<Dictionary<string, object>>> BarChartByUserConsumption(int id, DateTime startDate)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                usagePlans = usagePlans.Where(plan => plan.UsageStartDate >= startDate).ToList();
                var result = EnergyConsumptionAnalyzer.GenerateChart(usagePlans, null, EnergyConsumptionAnalyzer.ChartType.Bar);
                return new ServiceResponse<Dictionary<string, object>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting bar chart for user with id {id}", ex);
                return new ServiceResponse<Dictionary<string, object>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<Dictionary<string, object>>> ScatterChartByUserConsumption(int id, DateTime startDate)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                usagePlans = usagePlans.Where(plan => plan.UsageStartDate >= startDate).ToList();
                var result = EnergyConsumptionAnalyzer.GenerateChart(usagePlans, null, EnergyConsumptionAnalyzer.ChartType.Scatter);
                return new ServiceResponse<Dictionary<string, object>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting scatter chart for user with id {id}", ex);
                return new ServiceResponse<Dictionary<string, object>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<Dictionary<string, object>>> PieChartByUserConsumption(int id, DateTime startDate)
        {
            try
            {
                var usagePlans = await _repository.GetByUserAsync(id);
                usagePlans = usagePlans.Where(plan => plan.UsageStartDate >= startDate).ToList();

                List<Appliance> applianceList = await _applianceRepository.GetUserAppliancesAsync(id);
                var applianceIds = usagePlans.Select(u => u.ApplianceId).Distinct().ToList();
                applianceList = applianceList.Where(a => applianceIds.Contains(a.Id)).ToList();

                var result = EnergyConsumptionAnalyzer.GenerateChart(usagePlans, applianceList, EnergyConsumptionAnalyzer.ChartType.Pie);

                return new ServiceResponse<Dictionary<string, object>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting scatter chart for user with id {id}", ex);
                return new ServiceResponse<Dictionary<string, object>> { Success = false, ErrorMessage = ex.Message };
            }
        }


    }
}
