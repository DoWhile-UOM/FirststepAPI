namespace FirstStep.Models.DTOs
{
    public struct ViewCompanyListDto
    {
        public int company_id { get; set; }
        public required string company_name { get; set; }
        public required bool verification_status { get; set; }
        

    }
}