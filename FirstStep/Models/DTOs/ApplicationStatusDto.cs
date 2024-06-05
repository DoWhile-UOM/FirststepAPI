namespace FirstStep.Models.DTOs
{
    public struct ApplicationStatusDto
    {
        public required string status { get; set; }
        public string? cv_name { get; set; }
        public required DateTime submitted_date { get; set; }
    }
}
