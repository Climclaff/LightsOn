using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface ITownRepository
    {
        Task<Town> GetByIdAsync(int id);
        Task<List<Town>> GetAllAsync();
        Task<List<Town>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Town town);
        Task<bool> UpdateAsync(Town town);
        Task<bool> DeleteAsync(int id);
    }
}
