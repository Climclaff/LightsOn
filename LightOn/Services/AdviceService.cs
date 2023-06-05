using LightOn.BLL;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace LightOn.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IDistributedCache _cache;
        private readonly IAdviceRepository _repository;
        private readonly IApplianceUsageRepository _usageRepository;
        private readonly ILoggingService _logger;
        public AdviceService(IAdviceRepository repository, IDistributedCache cache, IApplianceUsageRepository usageRepository, ILoggingService logger)
        {
            _repository = repository;
            _usageRepository = usageRepository;
            _cache= cache;
            _logger = logger;
        }

        public async Task<ServiceResponse<ConcurrentDictionary<string, List<string>>>> GenerateTipsAsync(int id)
        {
            try
            {
                var dictionaryByteArray = await _cache.GetAsync(id.ToString() + "Tips");
                if (dictionaryByteArray != null)
                {
                    string json = Encoding.UTF8.GetString(dictionaryByteArray);
                    ConcurrentDictionary<string, List<string>> dictionary = JsonSerializer.Deserialize<ConcurrentDictionary<string, List<string>>>(json);
                    return new ServiceResponse<ConcurrentDictionary<string, List<string>>> { Success = true, Data = dictionary };
                }
                else
                {
                    var transf = await _repository.GetTransformerByUserAsync(id);
                    var users = await _repository.GetUsersByTransformerAsync(id);
                    var buildings = await _repository.GetBuildingsByUsersAsync(users);
                    var currentUser = users.Where(u => u.Id == id).FirstOrDefault();
                    ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages = new ConcurrentDictionary<User, List<ApplianceUsageHistory>>();
                    foreach (var user in users)
                    {
                        var result = await _usageRepository.GetByUserAsync(user.Id);
                        result = result.Where(x => x.UsageStartDate >= DateTime.Now - TimeSpan.FromDays(7)).ToList();
                        usages.TryAdd(user, result);
                    }

                    EnergyEfficiencyMeter efficiencyMeter = new EnergyEfficiencyMeter();
                    var tips = efficiencyMeter.GenerateEnergyEfficiencyTips(currentUser, usages, buildings);

                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromSeconds(900));
                    string jsonDictionary = JsonSerializer.Serialize(tips);
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonDictionary);
                    await _cache.SetAsync(id.ToString() + "Tips", byteArray, cacheEntryOptions);
                    return new ServiceResponse<ConcurrentDictionary<string, List<string>>> { Success = true, Data = tips };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ConcurrentDictionary<string, List<string>>> { Success = false, ErrorMessage = ex.Message };
            }
        }


    }
}
