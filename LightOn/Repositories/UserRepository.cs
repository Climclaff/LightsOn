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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public UserRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError($"Error deleting user from database with ID {id}", null);
                throw new NotFoundException($"User with id {id} not found.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Users.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"User with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding user with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving user with ID {id}", ex);
                throw new RepositoryException($"Error retrieving user with ID {id}", ex);
            }
        }

        public async Task<List<User>> GetRangeAsync(int offset, int count)
        {
            var totalUsers = await _context.Users.CountAsync();

            if (offset >= totalUsers)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of users {totalUsers}");
            }

            var users = await _context.Users.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalUsers - offset)).ToListAsync();

            return users;
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all users", ex);
                throw new RepositoryException("Failed to get all users", ex);
            }
        }

        public async Task<bool> ChangeImageAsync(int userId, byte[] imgData)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with id {userId} not found.");
                }
                user.ImageData = imgData;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload user image", ex);
                throw new RepositoryException("Failed to upload user image", ex);
            }
        }
    }
}
