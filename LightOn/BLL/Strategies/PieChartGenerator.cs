using LightOn.BLL.Interfaces;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL.Strategies
{
    public class PieChartGenerator : IChartGenerator
    {
        public ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances)
        {
            // Group the usage history by appliance ID and calculate the total energy consumed for each appliance
            var applianceEnergy = usageHistory
                .GroupBy(h => h.ApplianceId)
                .Select(g => new
                {
                    ApplianceId = g.Key,
                    EnergyConsumed = g.Sum(h => h.EnergyConsumed)
                })
                .ToList();

            var chartData = new ConcurrentDictionary<string, object>();
            chartData.TryAdd("labels", applianceEnergy.Select(a => GetApplianceName(a.ApplianceId, appliances)));
            chartData.TryAdd("data", applianceEnergy.Select(a => a.EnergyConsumed));
            return chartData;
        }

        private string GetApplianceName(int? applianceId, List<Appliance> applianceNames)
        {
            var appliance = applianceNames.FirstOrDefault(a => a.Id == applianceId);


            return appliance?.Name ?? string.Empty;
        }
    }
}
