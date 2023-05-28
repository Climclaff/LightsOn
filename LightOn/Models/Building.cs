using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Building
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? StreetId { get; set; }

        public int? Area { get; set; }

        [JsonIgnore]
        public Street? Street { get; set; }

        public int? TransformerId { get; set; }

        [JsonIgnore]
        public Transformer? Transformer { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
