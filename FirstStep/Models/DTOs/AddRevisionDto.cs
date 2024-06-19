namespace FirstStep.Models.DTOs
{
    public class AddRevisionDto
    {
        public required int application_id { get; set; }
        public string? comment { get; set; }
        public required string status { get; set; }
        public required int employee_id { get; set; }
    }
}
