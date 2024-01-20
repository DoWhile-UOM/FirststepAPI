using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class Company
    {
        [Key]
        public int company_id { get; set; }

        public required int business_reg_no { get; set; }

        public required string company_name { get; set; }

        public required string company_email { get; set; }

        public string? company_website { get; set; }

        public required int company_phone_number { get; set; }

        public required bool verification_status { get; set; }

        public string? business_reg_certificate { get; set; }

        public string? certificate_of_incorporation { get; set; }
    }
}
