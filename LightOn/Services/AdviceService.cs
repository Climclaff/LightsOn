using LightOn.BLL;
using LightOn.Helpers;
using LightOn.Models;
using LightOn.Repositories.Interfaces;
using LightOn.Services.Interfaces;
using System.Collections.Concurrent;

namespace LightOn.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceRepository _repository;
        private readonly IApplianceUsageRepository _usageRepository;
        private readonly ILoggingService _logger;
        public AdviceService(IAdviceRepository repository,IApplianceUsageRepository usageRepository, ILoggingService logger)
        {
            _repository = repository;
            _usageRepository = usageRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<ConcurrentDictionary<string, List<string>>>> GenerateTipsAsync(int id)
        {
            var transf = await _repository.GetTransformerByUserAsync(id);
            var users = await _repository.GetUsersByTransformerAsync(id);
            var buildings = await _repository.GetBuildingsByUsersAsync(users);
            var currentUser = users.Where(u => u.Id == id).FirstOrDefault();
            ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages = new ConcurrentDictionary<User, List<ApplianceUsageHistory>>();
            foreach(var user in users)
            {
                var result = await _usageRepository.GetByUserAsync(user.Id);
                result = result.Where(x => x.UsageStartDate >= DateTime.Now - TimeSpan.FromDays(7)).ToList();
                usages.TryAdd(user, result);
            }

            EnergyEfficiencyMeter efficiencyMeter = new EnergyEfficiencyMeter();
            var tips = efficiencyMeter.GenerateEnergyEfficiencyTips(currentUser, usages, buildings);
            return new ServiceResponse<ConcurrentDictionary<string, List<string>>> { Success = true, Data = tips };
        }


    }
}
