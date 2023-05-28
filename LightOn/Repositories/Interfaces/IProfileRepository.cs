using LightOn.Models;
using LightOn.Models.ClientModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightOn.Repositories.Interfaces
{
    public interface IProfileRepository 
    {
        Task<bool> ChangeLocation(int userId, int regionId, int districtId, int townId, int streetId, int buildingId);
        Task<bool> ChangeImageAsync(int userId, byte[] imgData);
        Task<bool> ChangeNameAsync(int userId, ChangeNameModel model);
        Task<bool> ChangeBuildingAreaAsync(int userId, int area);
        Task<List<Region>> GetRegionsAsync();
        Task<List<District>> GetDistrictsOfRegionAsync(int id);
        Task<List<Town>> GetTownsOfDistrictAsync(int id);
        Task<List<Street>> GetStreetsOfTownAsync(int id);
        Task<List<Building>> GetBuildingsOfStreetAsync(int id);


    }
}
