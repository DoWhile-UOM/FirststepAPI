namespace FirstStep.Models.DTOs
{
    public class UpdateRegistrationStatusDto
    {
        public int company_id { get; set; }
        public required bool verification_status { get; set; }
        public string? comment { get; set; }
    }
}
