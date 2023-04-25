using LightOn.Data;
using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightOn.Services
{
    public class TownService : ITownService
    {
        private readonly ITownRepository _repository;
        private readonly ILoggingService _logger;
        public TownService(ITownRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Town>> CreateAsync(Town town)
        {
            try
            {
                await _repository.CreateAsync(town);
                return new ServiceResponse<Town> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Town> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Town>> DeleteAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result == false)
                {
                    return new ServiceResponse<Town> { Success = false, ErrorMessage = $"Could not delete town with ID {id}" };
                }
                return new ServiceResponse<Town> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Town> { Success = false, ErrorMessage = ex.Message };
            }

        }


        public async Task<ServiceResponse<Town>> GetByIdAsync(int id)
        {
            try
            {
                var town = await _repository.GetByIdAsync(id);
                if (town == null)
                {
                    return new ServiceResponse<Town> { Success = false, ErrorMessage = $"Town with ID {id} was not found" };
                }
                return new ServiceResponse<Town> { Success = true, Data = town };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Town> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<Town>> UpdateAsync(Town town)
        {
            try
            {
                await _repository.UpdateAsync(town);
                return new ServiceResponse<Town> { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Town> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Town>>> GetRangeAsync(int offset, int count)
        {
            try
            {
                var towns = await _repository.GetRangeAsync(offset, count);
                return new ServiceResponse<List<Town>> { Success = true, Data = towns };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting towns with offeset {offset}", ex);
                return new ServiceResponse<List<Town>> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Town>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return new ServiceResponse<List<Town>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Town>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
