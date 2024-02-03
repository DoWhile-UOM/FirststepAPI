using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class Skill
    {
        [Key]
        public required int skill_id { get; set; }
        
        public required string skill_name { get; set; }


        public virtual ICollection<Seeker>? seekers { get; set; }
    }
}
