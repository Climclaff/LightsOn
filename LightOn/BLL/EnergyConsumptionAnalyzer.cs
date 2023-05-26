using LightOn.Models;

namespace LightOn.BLL
{
    public static class EnergyConsumptionAnalyzer
    {
        public static (int[] Counts, float[] Ranges) GenerateHistogram(List<ApplianceUsageHistory> usageHistory)
        {
            int numBins = (int)Math.Ceiling(Math.Sqrt(usageHistory.Count));

            float minEnergyConsumed = usageHistory.Min(h => h.EnergyConsumed);
            float maxEnergyConsumed = usageHistory.Max(h => h.EnergyConsumed);

            // Calculate the bin width
            float binWidth = (maxEnergyConsumed - minEnergyConsumed) / numBins;

            // Initialize the histogram data arrays
            int[] histogramCounts = new int[numBins];
            float[] histogramRanges = new float[numBins + 1];

            // Calculate the histogram ranges
            for (int i = 0; i <= numBins; i++)
            {
                histogramRanges[i] = minEnergyConsumed + (i * binWidth);
            }

            // Assign the energy consumption values to the histogram bins
            foreach (var history in usageHistory)
            {
                int binIndex = (int)((history.EnergyConsumed - minEnergyConsumed) / binWidth);
                if (binIndex < 0)
                    binIndex = 0;
                else if (binIndex >= numBins)
                    binIndex = numBins - 1;

                histogramCounts[binIndex]++;
            }

            return (histogramCounts, histogramRanges);
        }
        public static Dictionary<string, object> GenerateLineChart(List<ApplianceUsageHistory> usageHistory)
        {
            // Sort the usage history by the start date
            usageHistory = usageHistory.OrderBy(h => h.UsageStartDate).ToList();

            // Extract the dates and energy consumption values
            var dates = usageHistory.Select(h => h.UsageStartDate).ToArray();
            var energyConsumption = usageHistory.Select(h => h.EnergyConsumed).ToArray();

            // Create a dictionary to hold the chart data
            var chartData = new Dictionary<string, object>();

            // Add the data to the chart data dictionary
            chartData.Add("title", "Energy Consumption Over Time");
            chartData.Add("series", new[]
            {
        new
        {
            name = "Energy Consumption",
            xValues = dates,
            yValues = energyConsumption,
            chartType = "Line"
        }
    });
            chartData.Add("xAxis", new
            {
                label = "Date",
                min = dates.Min(),
                max = dates.Max()
            });
            chartData.Add("yAxis", new
            {
                label = "Energy Consumption (Watt)",
                min = energyConsumption.Min(),
                max = energyConsumption.Max()
            });

            return chartData;
        }

        public static Dictionary<string, object> GenerateBarChart(List<ApplianceUsageHistory> usageHistory)
        {
            // Sort the usage history by the start date
            usageHistory = usageHistory.OrderBy(h => h.UsageStartDate).ToList();

            // Extract the dates and energy consumption values
            var dates = usageHistory.Select(h => h.UsageStartDate).ToArray();
            var energyConsumption = usageHistory.Select(h => h.EnergyConsumed).ToArray();

            // Create a dictionary to hold the chart data
            var chartData = new Dictionary<string, object>();

            // Add the data to the chart data dictionary
            chartData.Add("title", "Energy Consumption by Date");
            chartData.Add("series", new[]
            {
        new
        {
            name = "Energy Consumption",
            xValues = dates,
            yValues = energyConsumption,
            chartType = "Bar"
        }
    });
            chartData.Add("xAxis", new
            {
                label = "Date",
                min = dates.Min(),
                max = dates.Max()
            });
            chartData.Add("yAxis", new
            {
                label = "Energy Consumption (Watt)",
                min = energyConsumption.Min(),
                max = energyConsumption.Max()
            });

            return chartData;
        }

        public static object GenerateScatterChart(List<ApplianceUsageHistory> usageHistory)
        {
            // Extract the dates and energy consumption values
            var dates = usageHistory.Select(h => h.UsageStartDate).ToList();
            var energyConsumption = usageHistory.Select(h => h.EnergyConsumed).ToList();
            float minEnergyConsumed = usageHistory.Min(h => h.EnergyConsumed);
            float maxEnergyConsumed = usageHistory.Max(h => h.EnergyConsumed);
            DateTime minDate = usageHistory.Min(h => h.UsageStartDate);
            DateTime maxDate = usageHistory.Max(h => h.UsageEndDate);
            var dataPoints = dates.Zip(energyConsumption, (date, energy) => new { X = date, Y = energy }).ToList();

            var chartData = new
            {
                minEnergyConsumed = minEnergyConsumed,
                maxEnergyConsumed = maxEnergyConsumed,
                minDate = minDate,
                maxDate = maxDate,
                DataPoints = dataPoints
            };

            return chartData;
        }

        public static object GeneratePieChart(List<ApplianceUsageHistory> usageHistory, List<Appliance> appliances)
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

            var chartData = new
            {
                Labels = applianceEnergy.Select(a => GetApplianceName(a.ApplianceId, appliances)),
                Data = applianceEnergy.Select(a => a.EnergyConsumed)
            };

            return chartData;
        }

        private static string GetApplianceName(int? applianceId, List<Appliance> applianceNames)
        {
            // Find the appliance name based on the appliance ID
            var appliance = applianceNames.FirstOrDefault(a => a.Id == applianceId);

            // If the appliance is found, return its name; otherwise, return an empty string or handle as desired
            return appliance?.Name ?? string.Empty;
        }
    }
}
