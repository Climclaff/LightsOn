using System.Text.Json.Serialization;

namespace LightOn.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime Date { get; set; }

        public int? UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
