using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<ServiceResponse<District>> GetByIdAsync(int id);
        Task<ServiceResponse<List<District>>> GetAllAsync();
        Task<ServiceResponse<List<District>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<District>> CreateAsync(District district);
        Task<ServiceResponse<District>> UpdateAsync(District district);
        Task<ServiceResponse<District>> DeleteAsync(int id);
    }
}
