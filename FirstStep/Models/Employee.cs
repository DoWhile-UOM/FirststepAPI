using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Employee : User
    {
        public bool is_HRM { get; set; }


        [JsonIgnore]
        public Company? company { get; set; }
        
        public required int company_id { get; set; }
    }
}
