using LightOn.BLL;
using LightOn.BLL.Interfaces;
using LightOn.BLL.Strategies;
using LightOn.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using static LightOn.BLL.EnergyConsumptionAnalyzer;
#pragma warning disable CS8600, CS8602
namespace TestBL
{

    public class Program
    {
        static void TestEnergyEfficientMeter()
        {
            User user1 = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                BuildingId = 1
            };

            User user2 = new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                BuildingId = 2
            };

            User user3 = new User
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Johnson",
                BuildingId = 3
            };


            Building building1 = new Building
            {
                Id = 1,
                Name = "Building 1",
                StreetId = 1,
                Area = 1000
            };

            Building building2 = new Building
            {
                Id = 2,
                Name = "Building 2",
                StreetId = 2,
                Area = 1500
            };

            Building building3 = new Building
            {
                Id = 3,
                Name = "Building 3",
                StreetId = 3,
                Area = 1200
            };

            // Create appliance usages for user1
            ApplianceUsageHistory usageHistory1 = new ApplianceUsageHistory
            {
                Id = 1,
                ApplianceId = 1,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 5.0f
            };

            ApplianceUsageHistory usageHistory2 = new ApplianceUsageHistory
            {
                Id = 2,
                ApplianceId = 2,
                UsageStartDate = new DateTime(2023, 5, 22, 10, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 22, 11, 0, 0),
                EnergyConsumed = 6.2f
            };

            ApplianceUsageHistory usageHistory3 = new ApplianceUsageHistory
            {
                Id = 3,
                ApplianceId = 3,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 7.2f
            };

            // Create appliance usages for user2
            ApplianceUsageHistory usageHistory4 = new ApplianceUsageHistory
            {
                Id = 4,
                ApplianceId = 4,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 4.5f
            };

            ApplianceUsageHistory usageHistory5 = new ApplianceUsageHistory
            {
                Id = 5,
                ApplianceId = 5,
                UsageStartDate = new DateTime(2023, 5, 22, 10, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 22, 11, 0, 0),
                EnergyConsumed = 5.8f
            };

            ApplianceUsageHistory usageHistory6 = new ApplianceUsageHistory
            {
                Id = 6,
                ApplianceId = 6,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 6.8f
            };

            // Create appliance usages for user3
            ApplianceUsageHistory usageHistory7 = new ApplianceUsageHistory
            {
                Id = 7,
                ApplianceId = 7,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 3.8f
            };

            ApplianceUsageHistory usageHistory8 = new ApplianceUsageHistory
            {
                Id = 8,
                ApplianceId = 8,
                UsageStartDate = new DateTime(2023, 5, 22, 10, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 22, 11, 0, 0),
                EnergyConsumed = 4.5f
            };

            ApplianceUsageHistory usageHistory9 = new ApplianceUsageHistory
            {
                Id = 9,
                ApplianceId = 9,
                UsageStartDate = new DateTime(2023, 5, 21, 9, 0, 0),
                UsageEndDate = new DateTime(2023, 5, 21, 10, 0, 0),
                EnergyConsumed = 5.5f
            };


            ConcurrentDictionary<User, List<ApplianceUsageHistory>> usageData = new ConcurrentDictionary<User, List<ApplianceUsageHistory>>();

            usageData.TryAdd(user1, new List<ApplianceUsageHistory> { usageHistory1, usageHistory2, usageHistory3 });
            usageData.TryAdd(user2, new List<ApplianceUsageHistory> { usageHistory4, usageHistory5, usageHistory6 });
            usageData.TryAdd(user3, new List<ApplianceUsageHistory> { usageHistory7, usageHistory8, usageHistory9 });

            List<Building> buildingList = new List<Building> { building1, building2, building3 };

            EnergyEfficiencyMeter efficiencyMeter = new EnergyEfficiencyMeter();

            ConcurrentDictionary<string, List<string>> tips = efficiencyMeter.GenerateEnergyEfficiencyTips(user3, usageData, buildingList);

            ConcurrentDictionary<string, List<string>> tips1 = efficiencyMeter.GenerateEnergyEfficiencyTips(user1, usageData, buildingList);
            ConcurrentDictionary<string, List<string>> tips2 = efficiencyMeter.GenerateEnergyEfficiencyTips(user2, usageData, buildingList);
            ConcurrentDictionary<string, List<string>> tips3 = efficiencyMeter.GenerateEnergyEfficiencyTips(user3, usageData, buildingList);
            Assert.IsTrue(tips1.TryGetValue("ReduceApplianceUsage", out List<string> applianceTips1));
            Console.WriteLine("Assertion for User 1: " + (applianceTips1.SequenceEqual(new List<string> { "3", "1", "2" }) ? "Passed" : "Failed"));


            Assert.IsTrue(tips2.TryGetValue("ReduceApplianceUsage", out List<string> applianceTips2));
            Console.WriteLine("Assertion for User 2: " + (applianceTips2.SequenceEqual(new List<string> { "6", "5", "4" }) ? "Passed" : "Failed"));

            Assert.IsTrue(tips3.TryGetValue("ReduceApplianceUsage", out List<string> applianceTips3));
            Console.WriteLine("Assertion for User 3: " + (applianceTips3.SequenceEqual(new List<string> { "9", "7", "8" }) ? "Passed" : "Failed"));
        }

        static void TestChart()
        {
            var usageHistory = new List<ApplianceUsageHistory>
    {
        new ApplianceUsageHistory { UsageStartDate = new DateTime(2023, 5, 1), EnergyConsumed = 100 },
        new ApplianceUsageHistory { UsageStartDate = new DateTime(2023, 5, 2), EnergyConsumed = 200 },
        new ApplianceUsageHistory { UsageStartDate = new DateTime(2023, 5, 3), EnergyConsumed = 150 },

    };

            var appliances = new List<Appliance>
    {
        new Appliance { Name = "Appliance 1" },
        new Appliance { Name = "Appliance 2" },
        new Appliance { Name = "Appliance 3" },

    };

            EnergyConsumptionAnalyzer analyzer = new EnergyConsumptionAnalyzer();

            // Act
            var chartData = analyzer.GenerateChart(usageHistory, appliances, ChartType.Bar);

            // Assert
            Assert.IsNotNull(chartData);
            Console.WriteLine("Assertion 1: chartData is not null. Result: " + (chartData != null));

            Assert.IsTrue(chartData.ContainsKey("title"));
            Console.WriteLine("Assertion 2: chartData contains 'title' key. Result: " + chartData.ContainsKey("title"));

            Assert.IsTrue(chartData.ContainsKey("series"));
            Console.WriteLine("Assertion 3: chartData contains 'series' key. Result: " + chartData.ContainsKey("series"));

            Assert.IsTrue(chartData.ContainsKey("xAxis"));
            Console.WriteLine("Assertion 4: chartData contains 'xAxis' key. Result: " + chartData.ContainsKey("xAxis"));

            Assert.IsTrue(chartData.ContainsKey("yAxis"));
            Console.WriteLine("Assertion 5: chartData contains 'yAxis' key. Result: " + chartData.ContainsKey("yAxis"));

            Assert.IsTrue(chartData.TryGetValue("title", out var title) && title != null);
            Console.WriteLine("Assertion 6: 'title' value is not null. Result: " + (chartData.TryGetValue("title", out var _) && title != null));

            Assert.IsTrue(chartData.TryGetValue("series", out var series) && series != null);
            Console.WriteLine("Assertion 7: 'series' value is not null. Result: " + (chartData.TryGetValue("series", out var _) && series != null));

            Assert.IsTrue(chartData.TryGetValue("xAxis", out var xAxis) && xAxis != null);
            Console.WriteLine("Assertion 8: 'xAxis' value is not null. Result: " + (chartData.TryGetValue("xAxis", out _) && xAxis != null));

            Assert.IsTrue(chartData.TryGetValue("yAxis", out var yAxis) && yAxis != null);
            Console.WriteLine("Assertion 9: 'yAxis' value is not null. Result: " + (chartData.TryGetValue("yAxis", out var _) && yAxis != null));

        }
        static void Main(string[] args)
        {
            TestEnergyEfficientMeter();
            TestChart();
        }
    }
}