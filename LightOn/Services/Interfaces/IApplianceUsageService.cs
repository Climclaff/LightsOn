using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IApplianceUsageService
    {
        Task<ServiceResponse<ApplianceUsageHistory>> GetByIdAsync(int id);
        Task<ServiceResponse<List<ApplianceUsageHistory>>> GetAllAsync();
        Task<ServiceResponse<List<ApplianceUsageHistory>>> GetByUserAsync(int id);
        Task<ServiceResponse<List<ApplianceUsageHistory>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<ApplianceUsageHistory>> CreateAsync(ApplianceUsageHistory usageHistory);
        Task<ServiceResponse<ApplianceUsageHistory>> UpdateAsync(ApplianceUsageHistory usageHistory);
        Task<ServiceResponse<ApplianceUsageHistory>> DeleteAsync(int id);
        Task<ServiceResponse<(int[], float[])>> HistogramByUserConsumption(int id);
        Task<ServiceResponse<Dictionary<string, object>>> LineChartByUserConsumption(int id);
        Task<ServiceResponse<Dictionary<string, object>>> BarChartByUserConsumption(int id);
        Task<ServiceResponse<object>> ScatterChartByUserConsumption(int id);
        Task<ServiceResponse<object>> PieChartByUserConsumption(int id);
    }
}
