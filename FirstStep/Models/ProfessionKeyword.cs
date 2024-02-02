using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class ProfessionKeyword
    {
        [Key]
        public int profession_id { get; set; }

        public required string profession_name { get; set; }

        
        public virtual JobField? job_Field { get; set; }
        
        public required int field_id { get; set; }


        [JsonIgnore]
        public virtual ICollection<Advertisement>? advertisements { get; set; }
    }
}
