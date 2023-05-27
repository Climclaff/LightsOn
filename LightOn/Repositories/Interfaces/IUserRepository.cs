using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetRangeAsync(int offset, int count);
        Task<bool> DeleteAsync(int id);
    }
}
