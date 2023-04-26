using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class TransformerService : ITransformerService
    {
        private readonly ITransformerRepository _repository;
        private readonly ILoggingService _logger;
        public TransformerService(ITransformerRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Transformer>> CreateAsync(Transformer transformer)
        {
            try
            {
                await _repository.CreateAsync(transformer);
                return new ServiceResponse<Transformer> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Transformer> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Transformer>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Transformer> { Success = false, ErrorMessage = $"Could not delete transformer with ID {id}" };
                }
                return new ServiceResponse<Transformer> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Transformer> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Transformer> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Transformer>> GetByIdAsync(int id)
        {
            try
            {
                var transformer = await _repository.GetByIdAsync(id);
                if (transformer == null)
                {
                    return new ServiceResponse<Transformer> { Success = false, ErrorMessage = $"Transformer with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<Transformer> { Success = true, Data = transformer };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Transformer> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Transformer> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Transformer>> UpdateAsync(Transformer transformer)
        {
            try
            {
                await _repository.UpdateAsync(transformer);
                return new ServiceResponse<Transformer> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Transformer> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Transformer> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Transformer>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var transformers = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Transformer>> { Success = true, Data = transformers };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting transformers with offeset {offset}", ex);
                return new ServiceResponse<List<Transformer>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Transformer>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Transformer>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Transformer>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
