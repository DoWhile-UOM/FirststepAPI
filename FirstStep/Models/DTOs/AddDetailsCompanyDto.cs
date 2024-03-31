using System.Text.Json.Serialization;

namespace FirstStep.Models.DTOs
{
    public struct AddDetailsCompanyDto
    {
        public required int company_id { get; set; }

        public string? company_logo { get; set; }

        public required string company_description { get; set; }

        public required string company_business_scale { get; set; }
    }
}
