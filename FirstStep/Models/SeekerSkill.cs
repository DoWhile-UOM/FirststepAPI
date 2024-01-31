using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class SeekerSkill
    {
        [Key]
        public required int seeker_Id { get; set; }
        
        public required string skillName { get; set; }
    }
}
