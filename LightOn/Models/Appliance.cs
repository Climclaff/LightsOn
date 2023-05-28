using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Appliance
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Power { get; set; }

        public float PowerFactor { get; set; }

        public int? UserId { get; set; }

        public float? Amperage { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<ApplianceUsageHistory>? ApplianceUsageHistories { get; set; }

        [JsonIgnore]
        public ICollection<ApplianceUsagePlanned>? ApplianceUsagePlanneds { get; set; }
    }
}
