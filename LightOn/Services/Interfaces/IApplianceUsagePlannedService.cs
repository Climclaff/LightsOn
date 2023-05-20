using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IApplianceUsagePlannedService
    {
        Task<ServiceResponse<ApplianceUsagePlanned>> GetByIdAsync(int id);
        Task<ServiceResponse<List<ApplianceUsagePlanned>>> GetAllAsync();
        Task<ServiceResponse<List<ApplianceUsagePlanned>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<ApplianceUsagePlanned>> CreateAsync(ApplianceUsagePlanned usagePlan);
        Task<ServiceResponse<ApplianceUsagePlanned>> UpdateAsync(ApplianceUsagePlanned usagePlan);
        Task<ServiceResponse<ApplianceUsagePlanned>> DeleteAsync(int id);
    }
}
