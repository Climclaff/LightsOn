using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILoggingService _logger;
        public UserService(IUserRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<ServiceResponse<User>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<User> { Success = false, ErrorMessage = $"Could not delete user with ID {id}" };
                }
                return new ServiceResponse<User> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<User> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<User> { Success = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ServiceResponse<User>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ServiceResponse<User> { Success = false, ErrorMessage = $"User with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<User> { Success = true, Data = user };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<User> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<User> { Success = false, ErrorMessage = ex.Message };
            }
        }


        public async Task<ServiceResponse<List<User>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var users = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<User>> { Success = true, Data = users };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting users with offeset {offset}", ex);
                return new ServiceResponse<List<User>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<User>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<User>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<User>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        
    }
}
