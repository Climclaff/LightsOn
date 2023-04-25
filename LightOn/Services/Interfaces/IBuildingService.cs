using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IBuildingService
    {
        Task<ServiceResponse<Building>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Building>>> GetAllAsync();
        Task<ServiceResponse<List<Building>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Building>> CreateAsync(Building building);
        Task<ServiceResponse<Building>> UpdateAsync(Building building);
        Task<ServiceResponse<Building>> DeleteAsync(int id);
    }
}
