using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Employee : User
    {
        public bool is_HRM { get; set; }


        [JsonIgnore]
        public RegisteredCompany? regCompany { get; set; }
        
        public required int company_id { get; set; }
    }
}
