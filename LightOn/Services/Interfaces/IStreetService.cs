using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IStreetService
    {
        Task<ServiceResponse<Street>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Street>>> GetAllAsync();
        Task<ServiceResponse<List<Street>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Street>> CreateAsync(Street street);
        Task<ServiceResponse<Street>> UpdateAsync(Street street);
        Task<ServiceResponse<Street>> DeleteAsync(int id);
    }
}
