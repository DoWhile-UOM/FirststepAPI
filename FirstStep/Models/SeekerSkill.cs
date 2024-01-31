using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class SeekerSkill
    {
        [Key]
        public int skillNo { get; set; }

        public required string skillName { get; set; }
    }
}
