namespace FirstStep.Models.DTOs
{
    public struct ApplicationListingPageDto
    {
        public string title { get; set; }

        public int job_number { get; set; }

        public string field_name { get; set; }

        public string current_status { get; set; }
        
        public IEnumerable<ApplicationListDto> applicationList { get; set; }
    }
}