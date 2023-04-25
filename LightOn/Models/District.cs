using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class District
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? RegionId { get; set; }

        [JsonIgnore]
        public Region? Region { get; set; }

        [JsonIgnore]
        public ICollection<Town>? Towns { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
