using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> GetByIdAsync(int id);
        Task<ServiceResponse<List<User>>> GetAllAsync();
        Task<ServiceResponse<List<User>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<User>> DeleteAsync(int id);
    }
}
