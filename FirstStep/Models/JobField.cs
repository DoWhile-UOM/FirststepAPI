using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class JobField
    {
        [Key]
        public int field_id { get; set; }

        public required string field_name { get; set; }


        [JsonIgnore]
        public virtual ICollection<ProfessionKeyword>? professionKeywords { get; set; }

        [JsonIgnore]
        public virtual ICollection<SeekerSkill>? seekerSkills { get; set; }

        [JsonIgnore]
        public virtual ICollection<Advertisement>? advertisements { get; set; }

        [JsonIgnore]
        public virtual ICollection<Seeker>? seekers { get; set; }
    }
}
