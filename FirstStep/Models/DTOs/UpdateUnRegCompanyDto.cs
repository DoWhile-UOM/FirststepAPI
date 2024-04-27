using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models.DTOs
{
    public class UpdateUnRegCompanyDto
    {
        public required int company_id { get; set; }

        public required int business_reg_no { get; set; }

        public required string company_name { get; set; }

        [EmailAddress]
        public required string company_email { get; set; }

        public string? company_website { get; set; }

        [DataType(DataType.PhoneNumber)]
        public required int company_phone_number { get; set; }

        public string? business_reg_certificate { get; set; }

        public string? certificate_of_incorporation { get; set; }

        public required DateTime company_applied_date { get; set; } = DateTime.Now;
    }
}
