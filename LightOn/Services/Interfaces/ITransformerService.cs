using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface ITransformerService
    {
        Task<ServiceResponse<Transformer>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Transformer>>> GetAllAsync();
        Task<ServiceResponse<List<Transformer>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Transformer>> CreateAsync(Transformer transformer);
        Task<ServiceResponse<Transformer>> UpdateAsync(Transformer transformer);
        Task<ServiceResponse<Transformer>> DeleteAsync(int id);
    }
}
