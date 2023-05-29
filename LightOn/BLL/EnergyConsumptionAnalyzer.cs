using LightOn.BLL.Interfaces;
using LightOn.BLL.Strategies;
using LightOn.Models;

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
            Scatter
        }
        private Dictionary<ChartType, IChartGenerator> chartGenerators = new Dictionary<ChartType, IChartGenerator>
        {
            { ChartType.Histogram, new HistogramChartGenerator() },
            { ChartType.Line, new LineChartGenerator() },
            { ChartType.Bar, new BarChartGenerator() },
            { ChartType.Pie, new PieChartGenerator() },
            { ChartType.Scatter, new ScatterChartGenerator() }
        };

        public Dictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance> appliances, ChartType chartType)
        {
            if (chartGenerators.ContainsKey(chartType))
            {
                // Get the corresponding chart generator
                var chartGen = chartGenerators[chartType];

                return chartGen.GenerateChart(usageHistory, appliances);

            }
            throw new ArgumentException("Unsupported chart Type");
        }
       
    }
}

        