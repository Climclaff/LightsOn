using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IPlanningPageRepository
    {
        Task<int?> GetTransformerByUserAsync(int id);
        Task<float> GetTransformerLoad(int id);
    }
}
