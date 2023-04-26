using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface ITransformerMeasurementService
    {
        Task<ServiceResponse<TransformerMeasurement>> GetByIdAsync(int id);
        Task<ServiceResponse<List<TransformerMeasurement>>> GetAllAsync();
        Task<ServiceResponse<List<TransformerMeasurement>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<TransformerMeasurement>> DeleteAsync(int id);
    }
}
