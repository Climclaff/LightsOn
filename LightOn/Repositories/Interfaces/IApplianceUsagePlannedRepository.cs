using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IApplianceUsagePlannedRepository
    {
        Task<ApplianceUsagePlanned> GetByIdAsync(int id);
        Task<List<ApplianceUsagePlanned>> GetAllAsync();
        Task<List<ApplianceUsagePlanned>> GetRangeAsync(int offset, int count);
        Task<List<ApplianceUsagePlanned>> GetByUserAsync(int id);
        Task<List<ApplianceUsagePlanned>> GetApplianceUsageForTransformer(int id);
        Task<bool> CreateAsync(ApplianceUsagePlanned usageHistory);
        Task<bool> UpdateAsync(ApplianceUsagePlanned usageHistory);
        Task<bool> DeleteAsync(int id);

    }
}
