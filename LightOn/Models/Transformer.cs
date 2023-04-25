using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Transformer
    {
        public int Id { get; set; }

        public int MaxLoad { get; set; }

        [JsonIgnore]
        public ICollection<Building>? Buildings { get; set; }

        [JsonIgnore]
        public ICollection<TransformerMeasurement>? TransformerMeasurements { get; set; }
    }
}
