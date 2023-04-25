using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IStreetRepository
    {
        Task<Street> GetByIdAsync(int id);
        Task<List<Street>> GetAllAsync();
        Task<List<Street>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Street street);
        Task<bool> UpdateAsync(Street street);
        Task<bool> DeleteAsync(int id);
    }
}
