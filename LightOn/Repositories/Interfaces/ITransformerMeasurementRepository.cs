using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface ITransformerMeasurementRepository
    {
        Task<TransformerMeasurement> GetByIdAsync(int id);
        Task<List<TransformerMeasurement>> GetAllAsync();
        Task<List<TransformerMeasurement>> GetRangeAsync(int offset, int count);
        Task<bool> DeleteAsync(int id);
    }
}
