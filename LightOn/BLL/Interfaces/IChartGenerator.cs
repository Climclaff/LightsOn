using LightOn.Models;
using System.Collections.Concurrent;

namespace LightOn.BLL.Interfaces
{
    public interface IChartGenerator
    {
        ConcurrentDictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances);
    }
}
