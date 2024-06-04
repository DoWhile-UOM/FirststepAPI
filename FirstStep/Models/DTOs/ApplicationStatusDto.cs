namespace FirstStep.Models.DTOs
{
    public class ApplicationStatusDto
    {
        
        public required string status { get; set; }
        public DateTime submitted_date { get; set; }
        public DateTime screening_date { get; set; }
        public DateTime finalize_date { get; set; }
        public required string cVurl { get; set; }


    }
}
