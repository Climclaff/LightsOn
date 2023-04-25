using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services
{
    public interface IRegionService
    {
        Task<ServiceResponse<Region>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Region>>> GetRegionsRangeAsync(int offset, int count);
        Task<ServiceResponse<Region>> CreateAsync(Region region);
        Task<ServiceResponse<Region>> UpdateAsync(Region region);
        Task<ServiceResponse<Region>> DeleteAsync(int id);
    }
}
