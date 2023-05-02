using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public int? RegionId { get; set; }

        [JsonIgnore]
        public Region? Region { get; set; }

        public int? DistrictId { get; set; }

        [JsonIgnore]
        public District? District { get; set; }

        public int? TownId { get; set; }

        [JsonIgnore]
        public Town? Town { get; set; }

        public int? StreetId { get; set; }

        [JsonIgnore]
        public Street? Street { get; set; }

        public int? BuildingId { get; set; }

        [JsonIgnore]
        public Building? Building { get; set; }

        [JsonIgnore]
        public ICollection<Appliance>? Appliances { get; set; }
        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; }


    }
}
