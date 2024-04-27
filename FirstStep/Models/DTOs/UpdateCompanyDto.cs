using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models.DTOs
{
    public class UpdateCompanyDto
    {
        public required int company_id { get; set; }

        public required string company_name { get; set; }

        [EmailAddress]
        public required string company_email { get; set; }

        public string? company_website { get; set; }

        [DataType(DataType.PhoneNumber)]
        public required int company_phone_number { get; set; }

        public string? company_logo { get; set; }

        public string? company_description { get; set; }

        public string? company_city { get; set; }

        public string? company_province { get; set; }

        public string? company_business_scale { get; set; }
    }
}
