using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IApplianceRepository
    {
        Task<Appliance> GetByIdAsync(int id);
        Task<List<Appliance>> GetAllAsync();
        Task<List<Appliance>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Appliance appliance);
        Task<bool> UpdateAsync(Appliance appliance);
        Task<bool> DeleteAsync(int id);
    }
}
