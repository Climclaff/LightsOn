using LightOn.Models;

namespace LightOn.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(int id);
        Task<List<Review>> GetAllAsync();
        Task<List<Review>> GetRangeAsync(int offset, int count);
        Task<bool> CreateAsync(Review review);
        Task<bool> DeleteAsync(int id);
    }
}
