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
        public required string experience { get; set; }
        public float? salary { get; set; }
        public string? currency_unit { get; set; }
        public DateTime submission_deadline { get; set; }
        public string? job_description { get; set; }
        public int field_id { get; set; }
        public ICollection<string>? reqKeywords { get; set; }
        public ICollection<string>? reqSkills { get; set; }
    }
}
