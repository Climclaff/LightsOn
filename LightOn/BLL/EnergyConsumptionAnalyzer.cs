using LightOn.BLL.Interfaces;
using LightOn.BLL.Strategies;
using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL
{
    public class EnergyConsumptionAnalyzer
    {
        public enum ChartType
        {
            Histogram,
            Line,
            Bar,
            Pie,
        }
        private Dictionary<ChartType, IChartGenerator> chartGenerators = new Dictionary<ChartType, IChartGenerator>
        {
            { ChartType.Histogram, new HistogramChartGenerator() },
            { ChartType.Line, new LineChartGenerator() },
            { ChartType.Bar, new BarChartGenerator() },
            { ChartType.Pie, new PieChartGenerator() },
        };

        public ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory,
            List<Appliance> appliances, ChartType chartType)
        {
            if (chartGenerators.ContainsKey(chartType))
            {
                var chartGen = chartGenerators[chartType];

                return chartGen.GenerateChart(usageHistory, appliances);

            }
            throw new ArgumentException("Unsupported chart Type");
        }
       
    }
}

        