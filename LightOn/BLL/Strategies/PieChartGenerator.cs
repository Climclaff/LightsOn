using LightOn.BLL.Interfaces;
using LightOn.Models;

namespace LightOn.BLL.Strategies
{
    public class PieChartGenerator : IChartGenerator
    {
        public Dictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances)
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

            var chartData = new Dictionary<string, object>
        {
            { "labels", applianceEnergy.Select(a => GetApplianceName(a.ApplianceId, appliances)) },
            { "data", applianceEnergy.Select(a => a.EnergyConsumed) }
        };

            return chartData;
        }

        private string GetApplianceName(int? applianceId, List<Appliance> applianceNames)
        {
            // Find the appliance name based on the appliance ID
            var appliance = applianceNames.FirstOrDefault(a => a.Id == applianceId);

            // If the appliance is found, return its name; otherwise, return an empty string or handle as desired
            return appliance?.Name ?? string.Empty;
        }
    }
}
