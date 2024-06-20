namespace FirstStep.Models.DTOs
{
    public class NotRegisteredEligibleCompanyDto
    {
        public int company_id { get; set; }
        public required string company_name { get; set; }
        public required string company_email { get; set; }
        public string? company_logo { get; set; }
    }
}
