using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IApplianceService
    {
        Task<ServiceResponse<Appliance>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Appliance>>> GetAllAsync();
        Task<ServiceResponse<List<Appliance>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Appliance>> CreateAsync(Appliance appliance);
        Task<ServiceResponse<Appliance>> UpdateAsync(Appliance appliance);
        Task<ServiceResponse<Appliance>> DeleteAsync(int id);
    }
}
