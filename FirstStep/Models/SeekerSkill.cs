using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class SeekerSkill
    {
        [Key]
        public required int skill_id { get; set; }
        
        public required string skill_name { get; set; }
    }
}
