using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;

namespace LightOn.Services
{
    public class TransformerMeasurementService : ITransformerMeasurementService
    {
        private readonly ITransformerMeasurementRepository _repository;
        private readonly ILoggingService _logger;
        public TransformerMeasurementService(ITransformerMeasurementRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<ServiceResponse<TransformerMeasurement>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<TransformerMeasurement> { Success = false, ErrorMessage = $"Could not delete transformer measurement with ID {id}" };
                }
                return new ServiceResponse<TransformerMeasurement> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<TransformerMeasurement> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<TransformerMeasurement> { Success = false, ErrorMessage = ex.Message };
            }

        }
        public async Task<ServiceResponse<TransformerMeasurement>> GetByIdAsync(int id)
        {
            try
            {
                var transformerMeasurement = await _repository.GetByIdAsync(id);
                if (transformerMeasurement == null)
                {
                    return new ServiceResponse<TransformerMeasurement> { Success = false, ErrorMessage = $"Transformer measurement with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<TransformerMeasurement> { Success = true, Data = transformerMeasurement };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<TransformerMeasurement> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<TransformerMeasurement> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<TransformerMeasurement>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var transformerMeasurements = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<TransformerMeasurement>> { Success = true, Data = transformerMeasurements };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting transformer measurements with offeset {offset}", ex);
                return new ServiceResponse<List<TransformerMeasurement>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<TransformerMeasurement>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<TransformerMeasurement>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<TransformerMeasurement>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}