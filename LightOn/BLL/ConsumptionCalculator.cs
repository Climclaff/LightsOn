namespace LightOn.BLL
{
    public static class ConsumptionCalculator
    {
        public static float Calculate(int wattPower, DateTime UsageStartDate, DateTime UsageEndDate)
        {
            var hours = ((float)(UsageEndDate - UsageStartDate).TotalHours);
            float totalEnergyConsumed = wattPower * hours;
            return totalEnergyConsumed;
        }
    }
}
