using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IBuildingRepository
    {
        Task<Building> GetByIdAsync(int id);
        Task<List<Building>> GetAllAsync();
        Task<List<Building>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Building building);
        Task<bool> UpdateAsync(Building building);
        Task<bool> DeleteAsync(int id);
    }
}
