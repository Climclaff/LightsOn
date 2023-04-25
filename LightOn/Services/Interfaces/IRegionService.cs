using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IRegionService
    {
        Task<ServiceResponse<Region>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Region>>> GetAllAsync();
        Task<ServiceResponse<List<Region>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Region>> CreateAsync(Region region);
        Task<ServiceResponse<Region>> UpdateAsync(Region region);
        Task<ServiceResponse<Region>> DeleteAsync(int id);
    }
}
