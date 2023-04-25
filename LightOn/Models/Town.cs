using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Town
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? DistrictId { get; set; }

        [JsonIgnore]
        public District? District { get; set; }

        [JsonIgnore]
        public ICollection<Street>? Streets { get; set; }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
