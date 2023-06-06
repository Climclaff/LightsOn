using LightOn.BLL.Interfaces;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL.Strategies
{
    public class HistogramChartGenerator : IChartGenerator
    { 
        public ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances)
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

            var chartData = new ConcurrentDictionary<string, object>();

            chartData.TryAdd("histogramCounts", histogramCounts);
            chartData.TryAdd("histogramRanges", histogramRanges);

            return chartData;
        }


    }
}
