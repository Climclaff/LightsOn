using LightOn.Helpers;
using LightOn.Models;

namespace LightOn.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResponse<Review>> GetByIdAsync(int id);
        Task<ServiceResponse<List<Review>>> GetAllAsync();
        Task<ServiceResponse<List<Review>>> GetRangeAsync(int offset, int count);
        Task<ServiceResponse<Review>> CreateAsync(Review review);
        Task<ServiceResponse<Review>> DeleteAsync(int id);
    }
}
