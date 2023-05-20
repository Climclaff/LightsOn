using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class ApplianceUsagePlanned
    {
        public int Id { get; set; }

        public DateTime UsageStartDate { get; set; }

        public DateTime UsageEndDate { get; set; }

        public float ApproximateLoad { get; set; }

        public int? ApplianceId { get; set; }

        [JsonIgnore]
        public Appliance? Appliance { get; set; }
    }
}
