using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Company
    {
        [Key]
        public int company_id { get; set; }

        public required int business_reg_no { get; set; }

        public required string company_name { get; set; }

        [EmailAddress]
        public required string company_email { get; set; }

        public string? company_website { get; set; }

        [DataType(DataType.PhoneNumber)]
        public required int company_phone_number { get; set; }

        public required bool verification_status { get; set; }

        public string? business_reg_certificate { get; set; }

        public string? certificate_of_incorporation { get; set; }

        public required DateTime company_applied_date { get; set; } = DateTime.Now;

        public string? company_logo { get; set; }

        public string? company_description { get; set; }

        public string? company_city { get; set; }

        public string? company_province { get; set; }

        public string? company_business_scale { get; set; }

        public string? comment { get; set;}

        public DateTime company_registered_date { get; set; }


        [JsonIgnore]
        public virtual SystemAdmin? verified_system_admin { get; set; }

        public int? verified_system_admin_id { get; set; }

        
        [JsonIgnore]
        public virtual HRManager? company_admin { get; set; }

        public int? company_admin_id { get; set; }

        
        [JsonIgnore]
        public ICollection<Employee>? employees { get; set; }

        //[JsonIgnore]
        //public ICollection<Advertisement>? advertisements { get; set; }
    }
}
