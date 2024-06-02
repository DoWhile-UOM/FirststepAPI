namespace FirstStep.Models.DTOs
{
    public class ApplicationStatusDto
    {
        public int application_Id { get; set; }
        public string status { get; set; }
        public DateTime submitted_date { get; set; }
        public DateTime screening_date { get; set; }
        public DateTime finalize_date { get; set; }

        //connect with revision to get the status
        public RevisionDto? last_revision { get; set; }

    }
}
