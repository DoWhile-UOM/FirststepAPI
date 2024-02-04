using System.Text.Json.Serialization;

namespace FirstStep.Models.DTOs
{
    public class RegisteredCompanyDto
    {
        public required int company_id { get; set; }

        public string? company_logo { get; set; }

        public required string company_description { get; set; }

        public required string company_city { get; set; }

        public required string company_province { get; set; }

        public required string company_business_scale { get; set; }

        public required DateTime company_registered_date { get; set; } = DateTime.UtcNow;

        public required int verified_system_admin_id { get; set; }
    }
}
