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
    public class TransformerRepository : ITransformerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public TransformerRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateAsync(Transformer transformer)
        {
            try
            {
                await _context.Transformers.AddAsync(transformer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding transformer to database.", ex);
                throw new RepositoryException("Error adding transformer", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transformer = await _context.Transformers.FindAsync(id);
            if (transformer == null)
            {
                _logger.LogError($"Error deleting transformer from database with ID {id}", null);
                throw new NotFoundException($"Transformer with id {id} not found.");
            }
            _context.Transformers.Remove(transformer);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Transformer> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Transformers.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Transformer with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding transformer with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving transformer with ID {id}", ex);
                throw new RepositoryException($"Error retrieving transformer with ID {id}", ex);
            }
        }

        public async Task<bool> UpdateAsync(Transformer transformer)
        {
            try
            {
                var result = await _context.Transformers.AsNoTracking().FirstOrDefaultAsync(current => current.Id == transformer.Id);
                if (result == null)
                {
                    throw new NotFoundException($"Transformer with id {transformer.Id} not found.");
                }
                _context.Entry(transformer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while updating transformer with ID {transformer.Id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating transformer with ID {transformer.Id}", ex);
                throw new RepositoryException($"Error updating transformer with ID {transformer.Id}", ex);
            }
        }

        public async Task<List<Transformer>> GetRangeAsync(int offset, int count)
        {
            var totalTransformers = await _context.Transformers.CountAsync();

            if (offset >= totalTransformers)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of transformers {totalTransformers}");
            }

            var transformers = await _context.Transformers.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalTransformers - offset)).ToListAsync();

            return transformers;
        }

        public async Task<List<Transformer>> GetAllAsync()
        {
            try
            {
                return await _context.Transformers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all transformers", ex);
                throw new RepositoryException("Failed to get all transformers", ex);
            }
        }
    }
}
