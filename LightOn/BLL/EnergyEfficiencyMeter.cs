using LightOn.Models;
using System.Collections.Concurrent;
//#pragma warning disable 8618, 8602, 8629
namespace LightOn.BLL
{
    public class EnergyEfficiencyMeter
    {

        private class ApplianceUsageScore
        {
            public ApplianceUsageHistory ApplianceUsage { get; set; }

            public double Score { get; set; }
        }


        public ConcurrentDictionary<string, List<string>> GenerateEnergyEfficiencyTips(User user, ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
        {
            ConcurrentDictionary<string, List<string>> tips = new ConcurrentDictionary<string, List<string>>();

            // Calculate user's energy consumption per square foot
            double userEnergyConsumptionPerSquareFoot = CalculateEnergyConsumptionPerSquareFoot(user, usages, buildings);
            double averageEnergyConsumptionPerSquareFoot = CalculateAverageEnergyConsumptionPerSquareFoot(usages, buildings);

            // Compare user's energy consumption per square foot with the average
            if (userEnergyConsumptionPerSquareFoot > averageEnergyConsumptionPerSquareFoot)
            {
                tips.TryAdd("EnergyConsumption", new List<string> { "Your energy consumption per square foot is higher than the average. Consider taking steps to improve energy efficiency." });
            }
            else
            {
                tips.TryAdd("EnergyConsumption", new List<string> { "Your energy consumption per square foot is within the average range. Keep up the good work!" });
            }

            // Identify heaviest appliance usages
            List<ApplianceUsageHistory> heaviestUsages = GetHeaviestApplianceUsages(usages);
            if (heaviestUsages.Any())
            {
                List<string> applianceTips = new List<string>();

                foreach (var usage in heaviestUsages)
                {
                    string tip = $"Reduce usage of {usage.ApplianceId} during peak energy demand hours.";
                    applianceTips.Add(tip);
                }

                tips.TryAdd("ApplianceUsage", applianceTips);
            }

            return tips;
        }

        public double CalculateEnergyConsumptionPerSquareFoot(User user, ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
        {
            var userUsageHistory = usages[user];
            var buildingId = user.BuildingId;
            var building = buildings.FirstOrDefault(b => b.Id == buildingId);
            var squareFootage = building.Area;

            var totalEnergyConsumption = userUsageHistory.Sum(usage => usage.EnergyConsumed);
            return totalEnergyConsumption / (float)squareFootage;
        }


        private List<ApplianceUsageHistory> GetHeaviestApplianceUsages(ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages)
        {
            var maxEnergyConsumption = usages.SelectMany(x => x.Value).Max(u => u.EnergyConsumed);
            var maxUsageDuration = usages.SelectMany(x => x.Value).Max(u => (u.UsageEndDate - u.UsageStartDate).TotalMinutes);

            // Calculate a score for each appliance usage based on energy consumption and usage duration
            List<ApplianceUsageScore> usageScores = usages.SelectMany(x => x.Value)
                .Select(usage => new ApplianceUsageScore
                {
                    ApplianceUsage = usage,
                    Score = CalculateApplianceUsageScore(usage, maxEnergyConsumption, maxUsageDuration)
                })
                .ToList();

            // Sort the appliance usages based on their scores in descending order
            List<ApplianceUsageHistory> heaviestUsages = usageScores.OrderByDescending(x => x.Score)
                .Select(x => x.ApplianceUsage)
                .Take(3)
                .ToList();

            return heaviestUsages;
        }
        private double CalculateApplianceUsageScore(ApplianceUsageHistory usage, float maximumEnergyConsumption, double maximumUsageDuration)
        {
            double energyWeight = 0.6;
            double durationWeight = 0.3;
            double peakUsageWeight = 0.1;

            // Normalize energy consumption and usage duration to a scale of 0 to 1
            double normalizedEnergyConsumption = usage.EnergyConsumed / maximumEnergyConsumption;
            double normalizedUsageDuration = (usage.UsageEndDate-usage.UsageStartDate).TotalMinutes / maximumUsageDuration;

            // Check if the appliance usage occurred during the peak load time
            // bool isDuringPeakLoadTime = usage.Timestamp.TimeOfDay >= peakLoadTime && usage.Timestamp.TimeOfDay <= peakLoadTimeEnd;

            // Assign a weight to the appliance usage based on its impact during peak load time
            // double peakUsageFactor = isDuringPeakLoadTime ? peakUsageWeight : 0;

            // Calculate the score by applying the weights to the normalized values
            double score = (energyWeight * normalizedEnergyConsumption) +
                           (durationWeight * normalizedUsageDuration); 
     //                      + peakUsageFactor;

            return score;
        }

        public double CalculateAverageEnergyConsumptionPerSquareFoot(ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
        {
            var totalEnergyConsumption = 0.0;
            var totalSquareFootage = 0.0;

            foreach (var user in usages.Keys)
            {
                var userUsageHistory = usages[user];
                var buildingId = user.BuildingId;
                var building = buildings.FirstOrDefault(b => b.Id == buildingId);
                var squareFootage = building.Area;

                totalEnergyConsumption += userUsageHistory.Sum(usage => usage.EnergyConsumed);
                totalSquareFootage += (double)squareFootage;
            }

            return totalEnergyConsumption / totalSquareFootage;
        }
    }
}
