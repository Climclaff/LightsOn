using LightOnBL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LightOnBL
{
    public class EnergyConsumptionAnalyzer
    {
        public int[] GenerateHistogram(List<ApplianceUsageHistory> usageHistory)
        {
            // number of bins determined from square root rule
            int numBins = (int)Math.Sqrt(usageHistory.Count);

            // Calculate the minimum and maximum energy consumed values
            float minEnergyConsumed = usageHistory.Min(h => h.EnergyConsumed);
            float maxEnergyConsumed = usageHistory.Max(h => h.EnergyConsumed);

            // Calculate the bin width
            float binWidth = (maxEnergyConsumed - minEnergyConsumed) / numBins;

            // Initialize the histogram data array
            int[] histogramData = new int[numBins];

            // Iterate over each usage history and increment the corresponding bin
            foreach (var history in usageHistory)
            {
                // Calculate the bin index for the energy consumed value
                int binIndex = (int)((history.EnergyConsumed - minEnergyConsumed) / binWidth);

                // Increment the bin count
                if (binIndex >= 0 && binIndex < numBins)
                    histogramData[binIndex]++;
            }

            return histogramData;
        }
    }
}
