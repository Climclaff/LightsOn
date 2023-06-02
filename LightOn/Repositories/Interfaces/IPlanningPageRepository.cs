using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IPlanningPageRepository
    {
        Task<int?> GetTransformerByUserAsync(int id);
    }
}
