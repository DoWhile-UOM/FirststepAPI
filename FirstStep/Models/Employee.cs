using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Employee : User
    {
        public required string role { get; set; }


        [JsonIgnore]
        public required RegisteredCompany regCompany { get; set; }
        
        public int company_id { get; set; }
    }
}
