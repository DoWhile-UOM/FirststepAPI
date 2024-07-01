using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    [Index(nameof(skill_name))]
    public class Skill
    {
        [Key]
        public required int skill_id { get; set; }

        [MaxLength(50)]
        public required string skill_name { get; set; }


        [JsonIgnore]
        public virtual ICollection<Seeker>? seekers { get; set; }

        [JsonIgnore]
        public virtual ICollection<Advertisement>? advertisements { get; set; }
    }
}
