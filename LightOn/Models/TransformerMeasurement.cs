using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class TransformerMeasurement
    {
        public int Id { get; set; }

        public float CurrentLoad { get; set; }

        public DateTime Date { get; set; }

        public int? TransformerId { get; set; }

        [JsonIgnore]
        public Transformer? Transformer { get; set; }
    }
}
