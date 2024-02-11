namespace FirstStep.Models.DTOs
{
    public class UpdateAdvertisementDto
    {
        public int advertisement_id { get; set; }
        public int job_number { get; set; }
        public required string title { get; set; }
        public required string country { get; set; }
        public required string city { get; set; }
        public required string employeement_type { get; set; }
        public required string arrangement { get; set; }
        public required bool is_experience_required { get; set; }
        public float salary { get; set; }
        public DateTime submission_deadline { get; set; }
        public string? job_overview { get; set; }
        public string? job_responsibilities { get; set; }
        public string? job_qualifications { get; set; }
        public string? job_benefits { get; set; }
        public string? job_other_details { get; set; }
        public int field_id { get; set; }
        public ICollection<string>? keywords { get; set; }
    }
}
