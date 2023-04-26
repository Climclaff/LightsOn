using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class StreetService : IStreetService
    {
        private readonly IStreetRepository _repository;
        private readonly ILoggingService _logger;
        public StreetService(IStreetRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Street>> CreateAsync(Street street)
        {
            try
            {
                await _repository.CreateAsync(street);
                return new ServiceResponse<Street> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Street> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Street>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Street> { Success = false, ErrorMessage = $"Could not delete street with ID {id}" };
                }
                return new ServiceResponse<Street> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Street> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Street> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Street>> GetByIdAsync(int id)
        {
            try
            {
                var street = await _repository.GetByIdAsync(id);
                if (street == null)
                {
                    return new ServiceResponse<Street> { Success = false, ErrorMessage = $"Street with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<Street> { Success = true, Data = street };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Street> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Street> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Street>> UpdateAsync(Street street)
        {
            try
            {
                await _repository.UpdateAsync(street);
                return new ServiceResponse<Street> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<Street> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Street> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Street>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var streets = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Street>> { Success = true, Data = streets };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting streets with offeset {offset}", ex);
                return new ServiceResponse<List<Street>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Street>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Street>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Street>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
