namespace FirstStep.Models.DTOs
{
    public struct AddAdvertisementDto
    {
        public int? job_number { get; set; }
        public string? title { get; set; }
        public string? country { get; set; }
        public string? city { get; set; }
        public string? employeement_type { get; set; }
        public string? arrangement { get; set; }
        public bool is_experience_required { get; set; }
        public float? salary { get; set; }
        public string? currency_unit { get; set; }
        public DateTime? submission_deadline { get; set; }
        public string? job_description { get; set; }
        public int? hrManager_id { get; set; }
        public int? field_id { get; set; }
        public ICollection<string>? keywords { get; set; }
        public ICollection<string>? reqSkills { get; set; }
    }
}
