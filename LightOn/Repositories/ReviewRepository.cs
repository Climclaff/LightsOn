using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8602 
#pragma warning disable CS8603 
namespace LightOn.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public ReviewRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Review review)
        {
            try
            {
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding review to database.", ex);
                throw new RepositoryException("Error adding review", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                _logger.LogError($"Error deleting review from database with ID {id}", null);
                throw new NotFoundException($"Review with id {id} not found.");
            }
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Review> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Reviews.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Review with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding review with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving review with ID {id}", ex);
                throw new RepositoryException($"Error retrieving review with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Review review)
        {
            try
            {
                var result = await _context.Reviews.FindAsync(review.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Review with id {review.Id} not found.");
                }
                _context.Entry(review).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating review with ID {review.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating review with ID {review.Id}", ex);
                throw new RepositoryException($"Error updating review with ID {review.Id}", ex);
            }
        }

        public async Task<List<Review>> GetRangeAsync(int offset, int count)
        {
            var totalReviews = await _context.Reviews.CountAsync();

            if (offset >= totalReviews)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of reviews {totalReviews}");
            }

            var reviews = await _context.Reviews.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalReviews - offset)).ToListAsync();

            return reviews;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            try
            {
                return await _context.Reviews.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all reviews", ex);
                throw new RepositoryException("Failed to get all reviews", ex);
            }
        }
    }
}
