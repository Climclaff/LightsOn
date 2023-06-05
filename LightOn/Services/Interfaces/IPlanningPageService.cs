using LightOn.Helpers;

namespace LightOn.Services.Interfaces
{
    public interface IPlanningPageService
    {
        Task<ServiceResponse<int?>> GetTransformerByUserAsync(int id);
        Task<ServiceResponse<float>> GetTransformerLoad(int id);
    }
}
