using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Street
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? TownId { get; set; }

        [JsonIgnore]
        public Town? Town { get; set; }

        [JsonIgnore]
        public ICollection<Building>? Buildings { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
