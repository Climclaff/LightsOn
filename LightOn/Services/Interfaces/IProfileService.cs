using LightOn.Helpers;
using LightOn.Models;
using LightOn.Models.ClientModels;

namespace LightOn.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ServiceResponse<bool>> ChangeLocationAsync(int userId, int regionId, int districtId, int townId, int streetId, int buildingId);
        Task<ServiceResponse<bool>> ChangeImageAsync(int userId, byte[] imgData);
        Task<ServiceResponse<bool>> ChangeNameAsync(int userId, ChangeNameModel model);
        Task<ServiceResponse<bool>> ChangeBuildingAreaAsync(int userId, int area);
        Task<ServiceResponse<List<Region>>> GetRegionsAsync();
        Task<ServiceResponse<List<District>>> GetDistrictsOfRegionAsync(int id);
        Task<ServiceResponse<List<Town>>> GetTownsOfDistrictAsync(int id);
        Task<ServiceResponse<List<Street>>> GetStreetsOfTownAsync(int id);
        Task<ServiceResponse<List<Building>>> GetBuildingsOfStreetAsync(int id);

        Task<ServiceResponse<Building>> GetUserBuilding(int id);
    }
}
