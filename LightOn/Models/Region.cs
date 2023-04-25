using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Region
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public ICollection<District>? Districts { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
