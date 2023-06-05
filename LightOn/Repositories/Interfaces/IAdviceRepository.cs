using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IAdviceRepository
    {
        Task<int?> GetTransformerByUserAsync(int id);
        Task<List<User>> GetUsersByTransformerAsync(int id);

        Task<List<Building>> GetBuildingsByUsersAsync(List<User> users);

    }
}
