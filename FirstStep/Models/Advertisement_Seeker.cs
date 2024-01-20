using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Advertisement_Seeker
    {
        [JsonIgnore]
        public required Advertisement advertisement { get; set; }

        public int advertisement_id { get; set; }
        
        
        [JsonIgnore]
        public required Seeker seeker { get; set; }

        public int seeker_id { get; set; }
    }
}
