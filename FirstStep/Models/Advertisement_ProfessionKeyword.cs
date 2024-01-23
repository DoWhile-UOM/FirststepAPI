using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Advertisement_ProfessionKeyword
    {
        [JsonIgnore]
        public virtual Advertisement? advertisement { get; set; }
        
        public int advertisement_id { get; set; }

        
        [JsonIgnore]
        public virtual ProfessionKeyword? professionKeyword { get; set; }
        
        public int profession_id { get; set; }        
    }
}
