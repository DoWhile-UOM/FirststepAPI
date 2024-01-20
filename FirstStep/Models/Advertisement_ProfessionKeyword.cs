using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Advertisement_ProfessionKeyword
    {
        [JsonIgnore]
        public required Advertisement advertisement { get; set; }
        
        public int advertisement_id { get; set; }

        
        [JsonIgnore]
        public required ProfessionKeyword professionKeyword { get; set; }
        
        public int profession_id { get; set; }        
    }
}
