using LightOn.Models;

namespace LightOn.BLL.Interfaces
{
    public interface IChartGenerator
    {
        Dictionary<string, object> GenerateChart(List<ApplianceUsageHistory> usageHistory, List<Appliance>? appliances);
    }
}
