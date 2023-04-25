using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface ITransformerRepository
    {
        Task<Transformer> GetByIdAsync(int id);
        Task<List<Transformer>> GetAllAsync();
        Task<List<Transformer>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Transformer transformer);
        Task<bool> UpdateAsync(Transformer transformer);
        Task<bool> DeleteAsync(int id);
    }
}
