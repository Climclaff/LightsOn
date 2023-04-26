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
    public class TransformerMeasurementRepository : ITransformerMeasurementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggingService _logger;
        public TransformerMeasurementRepository(ApplicationDbContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transformerMeasurement = await _context.TransformerMeasurements.FindAsync(id);
            if (transformerMeasurement == null)
            {
                _logger.LogError($"Error deleting transformer measurement from database with ID {id}", null);
                throw new NotFoundException($"Transformer measurement with id {id} not found.");
            }
            _context.TransformerMeasurements.Remove(transformerMeasurement);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<TransformerMeasurement> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.TransformerMeasurements.FindAsync(id);
                if (result == null)
                {
                    throw new NotFoundException($"Transformer measurement with id {id} not found.");
                }
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"An error occurred while finding transformer measurement with ID {id}", ex);
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving transformer measurement with ID {id}", ex);
                throw new RepositoryException($"Error retrieving transformer measurement with ID {id}", ex);
            }
        }
        public async Task<List<TransformerMeasurement>> GetRangeAsync(int offset, int count)
        {
            var totalTransformerMeasurements = await _context.TransformerMeasurements.CountAsync();

            if (offset >= totalTransformerMeasurements)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"Offset {offset} exceeds the total number of transformer measurements {totalTransformerMeasurements}");
            }

            var transformerMeasurements = await _context.TransformerMeasurements.OrderBy(r => r.Id).Skip(offset).Take(Math.Min(count, totalTransformerMeasurements - offset)).ToListAsync();

            return transformerMeasurements;
        }
        public async Task<List<TransformerMeasurement>> GetAllAsync()
        {
            try
            {
                return await _context.TransformerMeasurements.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all transformer measurements", ex);
                throw new RepositoryException("Failed to get all transformer measurements", ex);
            }
        }
    }
}
