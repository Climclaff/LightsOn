using LightOn.Models;

namespace LightOn.Repositories
{
    public interface IRegionRepository
    {
        Task<Region> GetByIdAsync(int id);
        Task<List<Region>> GetRegionsRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Region region);
        Task<bool> UpdateAsync(Region region);
        Task<bool> DeleteAsync(int id);
    }
}
