namespace FirstStep.Models.DTOs
{
    public class ApplicationStatusDto
    {
        
        public required string status { get; set; }
        public string? cv_name { get; set; }//make this required
        //submited date
        public required DateTime submitted_date { get; set; }


    }
}
