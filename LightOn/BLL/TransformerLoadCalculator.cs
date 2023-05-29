using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL
{
    public class TransformerLoadCalculator
    {

        public ConcurrentDictionary<DateTime, float> GenerateSegments(Transformer transformer, List<ApplianceUsagePlanned> usagePlanned, List<Appliance> appliances)
        {
            ConcurrentDictionary<DateTime, float> segments = new ConcurrentDictionary<DateTime, float>();

            DateTime now = DateTime.Now;
            DateTime endDateTime = now.AddDays(1); 

            // Round the start time to the nearest 15-minute interval
            DateTime startTime = now.AddMinutes(15 - (now.Minute % 15));

            // Generate the 15-minute segments
            try
            {
                while (startTime < endDateTime)
                {
                    List<int?> applianceIdsInSegment = usagePlanned
                        .Where(u => u.UsageStartDate <= startTime.AddMinutes(15) && u.UsageEndDate >= startTime)
                        .Select(u => u.ApplianceId)
                        .ToList();
                    startTime = startTime.AddMinutes(15);

                    List<Appliance> appliancesInSegment = appliances
                    .Where(a => applianceIdsInSegment.Contains(a.Id))
                    .ToList();

                    // Calculate the load percentage for the current segment
                    float loadPercentage = CalculateTransformerLoadPercentage(transformer, appliancesInSegment);

                    // Add the segment to the dictionary
                    segments.TryAdd(startTime, loadPercentage);
                }
                return segments;
            }
            catch(Exception)
            {
                throw new Exception("Error during transformer load calculation");
            }
        }
        private float CalculateTransformerLoadPercentage(Transformer transformer, List<Appliance> appliances)
        {
            float totalPercentage = 0;
            foreach (var appliance in appliances) {
                float kWtPower = (float)appliance.Power / 1000;
                float apparentPower = kWtPower / appliance.PowerFactor;
                float loadPercentage = (apparentPower / transformer.MaxLoad) * 100;
                totalPercentage += loadPercentage;
            }
            return totalPercentage;
        }

    }
}
