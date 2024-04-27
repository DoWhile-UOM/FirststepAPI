namespace FirstStep.Models.DTOs
{
    public class CompanyRegInfoDto
    {
        public required int company_id { get; set; }

        public bool verification_status { get; set; } = false;

        public required DateTime company_registered_date { get; set; } = DateTime.UtcNow;

        public required int verified_system_admin_id { get; set; }

        public string? comment { get; set; }
    }
}
