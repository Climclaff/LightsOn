using LightOn.BLL.Interfaces;
using LightOn.Migrations;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL.Strategies
{
    public class LineChartGenerator : IChartGenerator
    {
        public ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances)
        {
            // Sort the usage history by the start date
            usageHistory = usageHistory.OrderBy(h => h.UsageStartDate).ToList();

            // Extract the dates and energy consumption values
            var dates = usageHistory.Select(h => h.UsageStartDate).ToArray();
            var energyConsumption = usageHistory.Select(h => h.EnergyConsumed).ToArray();

            // Create a dictionary to hold the chart data
            var chartData = new ConcurrentDictionary<string, object>();

            // Add the data to the chart data dictionary
            chartData.TryAdd("title", "Energy Consumption Over Time");
            chartData.TryAdd("series", new[]
            {
                new
                {
                    name = "Energy Consumption",
                    xValues = dates,
                    yValues = energyConsumption,
                    chartType = "Line"
                }
            });
            chartData.TryAdd("xAxis", new
            {
                label = "Date",
                min = dates.Min(),
                max = dates.Max()
            });
            chartData.TryAdd("yAxis", new
            {
                label = "Energy Consumption (Watt)",
                min = energyConsumption.Min(),
                max = energyConsumption.Max()
            });

            return chartData;
        }
    }
}
