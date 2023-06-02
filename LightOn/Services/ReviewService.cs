using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly ILoggingService _logger;
        public ReviewService(IReviewRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Review>> CreateAsync(Review review)
        {
            try
            {
                await _repository.CreateAsync(review);
                return new ServiceResponse<Review> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Review> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Review>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Review> { Success = false, ErrorMessage = $"Could not delete review with ID {id}" };
                }
                return new ServiceResponse<Review> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Review> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Review> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Review>> GetByIdAsync(int id)
        {
            try
            {
                var review = await _repository.GetByIdAsync(id);
                if (review == null)
                {
                    return new ServiceResponse<Review> { Success = false, ErrorMessage = $"Review with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<Review> { Success = true, Data = review };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Review> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Review> { Success = false, ErrorMessage = ex.Message };
            }
        }



        public async Task<ServiceResponse<List<Review>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var reviews = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Review>> { Success = true, Data = reviews };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting reviews with offeset {offset}", ex);
                return new ServiceResponse<List<Review>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Review>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Review>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Review>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
