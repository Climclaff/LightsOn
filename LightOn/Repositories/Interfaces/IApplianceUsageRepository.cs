using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IApplianceUsageRepository
    {
        Task<ApplianceUsageHistory> GetByIdAsync(int id);
        Task<List<ApplianceUsageHistory>> GetByUserAsync(int id);
        Task<List<ApplianceUsageHistory>> GetAllAsync();
        Task<List<ApplianceUsageHistory>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(ApplianceUsageHistory usageHistory);
        Task<bool> UpdateAsync(ApplianceUsageHistory usageHistory);
        Task<bool> DeleteAsync(int id);
    }
}
