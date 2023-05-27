using LightOn.Exceptions;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Models.ClientModels;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;

namespace LightOn.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repository;
        private readonly ILoggingService _logger;
        public ProfileService(IProfileRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> ChangeLocation(int userId, int regionId, int districtId, int townId, int streetId, int buildingId)
        {
            try
            {
                await _repository.ChangeLocation(userId, regionId, districtId, townId, streetId, buildingId);
                return new ServiceResponse<bool> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<bool> { Success = false, NotFound = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<bool>> ChangeImageAsync(int userId, byte[] imgData)
        {
            try
            {
                var result = await _repository.ChangeImageAsync(userId, imgData);
                return new ServiceResponse<bool> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<bool> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> ChangeNameAsync(int userId, ChangeNameModel model)
        {
            try
            {
                var result = await _repository.ChangeNameAsync(userId, model);
                return new ServiceResponse<bool> { Success = true };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<bool> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<ServiceResponse<List<Building>>> GetBuildingsOfStreetAsync(int id)
        {
            try
            {
                var result = await _repository.GetBuildingsOfStreetAsync(id);
                if (result == null)
                {
                    return new ServiceResponse<List<Building>> { Success = false, ErrorMessage = $"Street with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<List<Building>> { Success = true, Data = result };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<List<Building>> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Building>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<District>>> GetDistrictsOfRegionAsync(int id)
        {
            try
            {
                var result = await _repository.GetDistrictsOfRegionAsync(id);
                if (result == null)
                {
                    return new ServiceResponse<List<District>> { Success = false, ErrorMessage = $"Region with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<List<District>> { Success = true, Data = result };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<List<District>> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<District>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Region>>> GetRegionsAsync()
        {
            try
            {
                var result = await _repository.GetRegionsAsync();
                return new ServiceResponse<List<Region>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Region>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Street>>> GetStreetsOfTownAsync(int id)
        {
            try
            {
                var result = await _repository.GetStreetsOfTownAsync(id);
                if (result == null)
                {
                    return new ServiceResponse<List<Street>> { Success = false, ErrorMessage = $"Town with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<List<Street>> { Success = true, Data = result };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<List<Street>> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Street>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ServiceResponse<List<Town>>> GetTownsOfDistrictAsync(int id)
        {
            try
            {
                var result = await _repository.GetTownsOfDistrictAsync(id);
                if (result == null)
                {
                    return new ServiceResponse<List<Town>> { Success = false, ErrorMessage = $"District with ID {id} was not found", NotFound = true };
                }
                return new ServiceResponse<List<Town>> { Success = true, Data = result };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResponse<List<Town>> { Success = false, ErrorMessage = ex.Message, NotFound = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Town>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
