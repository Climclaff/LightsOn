using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IDistrictRepository
    {
        Task<District> GetByIdAsync(int id);
        Task<List<District>> GetAllAsync();
        Task<List<District>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(District district);
        Task<bool> UpdateAsync(District district);
        Task<bool> DeleteAsync(int id);
    }
}
