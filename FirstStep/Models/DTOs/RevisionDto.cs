namespace FirstStep.Models.DTOs
{
    public class RevisionDto
    {
        public int revision_id { get; set; }

        public string? comment { get; set; }

        public required string status { get; set; }

        public DateTime created_date { get; set; }

        public required int employee_id { get; set; }

    }
}
