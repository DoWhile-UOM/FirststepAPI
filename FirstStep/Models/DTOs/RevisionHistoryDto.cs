namespace FirstStep.Models.DTOs
{
    public class RevisionHistoryDto
    {
        public int revision_id { get; set; }
        public string? comment { get; set; }
        public required string status { get; set; }
        public DateTime created_date { get; set; }
        public required string employee_name { get; set; }
        public required string employee_role { get; set; }
    }
}
