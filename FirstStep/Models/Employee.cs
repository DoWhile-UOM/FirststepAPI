using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Employee : User
    {
        [JsonIgnore]
        public Company? company { get; set; }
        
        public required int company_id { get; set; }


        [JsonIgnore]
        public virtual ICollection<Revision>? revisions { get; set; }
    }
}
