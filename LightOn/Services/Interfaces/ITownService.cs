using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface ITownService
    {
        Task<ServiceResponse<Town>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Town>>> GetAllAsync();
        Task<ServiceResponse<List<Town>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Town>> CreateAsync(Town town);
        Task<ServiceResponse<Town>> UpdateAsync(Town town);
        Task<ServiceResponse<Town>> DeleteAsync(int id);
    }
}
