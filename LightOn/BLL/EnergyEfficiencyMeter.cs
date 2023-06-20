using LightOn.Models;
using System.Collections.Concurrent;
using System.Linq;
#pragma warning disable 8618, 8602, 8629
namespace LightOn.BLL
{
    public class EnergyEfficiencyMeter
    {

        private class ApplianceUsageScore
        {
            public ApplianceUsageHistory ApplianceUsage { get; set; }

            public double Score { get; set; }
        }
        public TimeSpan PeakLoadHour { get; set; }

        public ConcurrentDictionary<string, List<string>> GenerateEnergyEfficiencyTips(User user, ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
        {
            try
            {
                ConcurrentDictionary<string, List<string>> tips = new ConcurrentDictionary<string, List<string>>();

                // Calculate user's energy consumption per square foot
                double userEnergyConsumptionPerSquareFoot = CalculateEnergyConsumptionPerSquareFoot(user, usages, buildings);
                double averageEnergyConsumptionPerSquareFoot = CalculateAverageEnergyConsumptionPerSquareFoot(usages, buildings);

                // Compare user's energy consumption per square foot with the average
                if (userEnergyConsumptionPerSquareFoot > averageEnergyConsumptionPerSquareFoot)
                {
                    tips.TryAdd("EnergyConsumption", new List<string> { $"Your energy consumption per square foot is " +
                        $"{(((userEnergyConsumptionPerSquareFoot-averageEnergyConsumptionPerSquareFoot)/averageEnergyConsumptionPerSquareFoot)*100).ToString("0.00")} " +
                        $"% higher than the average. Consider taking steps to improve energy efficiency." });
                }
                else
                {
                    tips.TryAdd("EnergyConsumption", new List<string> { "Your energy consumption per square foot is within the average range. Keep up the good work!" });
                }
                SetPeakUsageTime(usages);
                // Identify heaviest appliance usages
                List<ApplianceUsageHistory> heaviestUsages = GetHeaviestApplianceUsages(user, usages);
                if (heaviestUsages.Any())
                {
                    List<string> applianceTips = new List<string>();

                    foreach (var usage in heaviestUsages)
                    {
                        string tip = $"{usage.ApplianceId}";
                        applianceTips.Add(tip);
                    }

                    tips.TryAdd("ReduceApplianceUsage", applianceTips);
                }

                return tips;
            }
            catch (Exception)
            {
                throw new Exception("An error occured during generating tips. The data provided to the system is insufficient for generation");
            }
        }

        private double CalculateEnergyConsumptionPerSquareFoot(User user, ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
        {
            var userUsageHistory = usages[user];
            var buildingId = user.BuildingId;
            var building = buildings.FirstOrDefault(b => b.Id == buildingId);
            var squareFootage = building.Area;

            var totalEnergyConsumption = userUsageHistory.Sum(usage => usage.EnergyConsumed);
            return totalEnergyConsumption / (float)squareFootage;
        }


        private List<ApplianceUsageHistory> GetHeaviestApplianceUsages(User user, ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages)
        {
            var userUsages = usages[user];
            var maxEnergyConsumption = usages.SelectMany(x => x.Value).Max(u => u.EnergyConsumed);
            var maxUsageDuration = usages.SelectMany(x => x.Value).Max(u => (u.UsageEndDate - u.UsageStartDate).TotalMinutes);

            // Calculate a score for each appliance usage based on energy consumption and usage duration
            List<ApplianceUsageScore> usageScores = userUsages.Select(usage => new ApplianceUsageScore
            {
                ApplianceUsage = usage,
                Score = CalculateApplianceUsageScore(usage, maxEnergyConsumption, maxUsageDuration)
            }).DistinctBy(x => x.ApplianceUsage.ApplianceId)
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
            double normalizedUsageDuration = (usage.UsageEndDate - usage.UsageStartDate).TotalMinutes / maximumUsageDuration;



            // Check if the appliance usage occurred during the peak load time
            bool isDuringPeakLoadTime = usage.UsageStartDate.TimeOfDay >= PeakLoadHour && usage.UsageEndDate.TimeOfDay <= PeakLoadHour.Add(TimeSpan.FromHours(1));

            // Assign a weight to the appliance usage based on its impact during peak load time
            double peakUsageFactor = isDuringPeakLoadTime ? peakUsageWeight : 0;

            // Calculate the score by applying the weights to the normalized values
            double score = (energyWeight * normalizedEnergyConsumption) +
                           (durationWeight * normalizedUsageDuration) +
                            peakUsageFactor;

            return score;
        }

        private double CalculateAverageEnergyConsumptionPerSquareFoot(ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages, List<Building> buildings)
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

        private void SetPeakUsageTime(ConcurrentDictionary<User, List<ApplianceUsageHistory>> usages)
        {
            var peakHourGroups = usages.SelectMany(u => u.Value)
                                 .GroupBy(u => u.UsageStartDate.Hour)
                                 .Select(g => new
                                 {
                                     Hour = g.Key,
                                     TotalEnergyConsumed = g.Sum(u => u.EnergyConsumed)
                                 })
                                 .OrderByDescending(g => g.TotalEnergyConsumed);

            var peakHourGroup = peakHourGroups.FirstOrDefault();

            if (peakHourGroup != null)
            {
                PeakLoadHour = TimeSpan.FromHours(peakHourGroup.Hour);
            }
            else
            {
                PeakLoadHour = TimeSpan.FromHours(18); // Default peak hour (6 PM)
            }
        }
    }
}
