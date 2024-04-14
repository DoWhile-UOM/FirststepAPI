namespace FirstStep.Models.DTOs
{
    public struct AdvertisementDto
    {
        public int? job_number { get; set; }
        public required string title { get; set; }
        public required string country { get; set; }
        public required string city { get; set; }
        public required string employeement_type { get; set; }
        public required string arrangement { get; set; }
        public required bool is_experience_required { get; set; }
        public required DateTime posted_date { get; set; }
        public float? salary { get; set; }
        public string? currency_unit { get; set; }
        public DateTime submission_deadline { get; set; }
        public string? job_description { get; set; }
        public string? company_name { get; set; }
        public string? field_name { get; set; }
        public ICollection<Skill> skills { get; set; }
    }
}
