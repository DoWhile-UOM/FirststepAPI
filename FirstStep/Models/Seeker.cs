using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Seeker : User
    {
        public int phone_number { get; set; }

        public required string bio { get; set; }

        public required string description { get; set; }

        public string? university { get; set; }

        public required string CVurl { get; set; }

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }


        [JsonIgnore]
        public virtual ICollection<Advertisement>? savedAdvertisemnts { get; set; }
    }
}
