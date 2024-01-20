using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class RegisteredCompany : Company
    {
        public string? company_logo { get; set; }

        public required string company_description { get; set; }

        public required string company_city { get; set; }

        public required string company_province { get; set; }

        public required string company_business_scale { get; set; }

        public required DateTime company_registered_date { get; set; }


        [JsonIgnore]
        public required ICollection<Employee> employees { get; set; }
    }
}
