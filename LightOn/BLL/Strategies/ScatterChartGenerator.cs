using LightOn.BLL.Interfaces;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL.Strategies
{
    public class ScatterChartGenerator : IChartGenerator
    {
        public ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances)
        {
            var dates = usageHistory.Select(h => h.UsageStartDate).ToList();
            var energyConsumption = usageHistory.Select(h => h.EnergyConsumed).ToList();
            float minEnergyConsumed = usageHistory.Min(h => h.EnergyConsumed);
            float maxEnergyConsumed = usageHistory.Max(h => h.EnergyConsumed);
            DateTime minDate = usageHistory.Min(h => h.UsageStartDate);
            DateTime maxDate = usageHistory.Max(h => h.UsageEndDate);
            var dataPoints = dates.Zip(energyConsumption, (date, energy) => new { X = date, Y = energy }).ToList();

            var chartData = new ConcurrentDictionary<string, object>();
            chartData.TryAdd("minEnergyConsumed ", minEnergyConsumed);
            chartData.TryAdd("maxEnergyConsumed ", maxEnergyConsumed);
            chartData.TryAdd("minDate ", minDate);
            chartData.TryAdd("maxDate ", maxDate);
            chartData.TryAdd("data", dataPoints);
            

            return chartData;
        }
    }
}
