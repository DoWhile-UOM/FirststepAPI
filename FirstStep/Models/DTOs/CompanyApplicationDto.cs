namespace FirstStep.Models.DTOs
    // this dto is to view company application in system admin portal
{
    public class CompanyApplicationDto
    {
        public int company_id { get; set; }
        public required int business_reg_no { get; set; }
        public required string company_name { get; set; }
        public required bool verification_status { get; set; }
        public required string company_email { get; set; }
        public string? company_website { get; set; }
        public required int company_phone_number { get; set; }
        public string? business_reg_certificate { get; set; }
        public string? certificate_of_incorporation { get; set; }
        public string? comment { get; set; }
        public required int verified_system_admin_id { get; set; }
        public string? company_business_scale { get; set; }
        public string? company_logo { get; set; }
    }
}
