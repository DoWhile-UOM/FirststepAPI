﻿namespace FirstStep.Models.DTOs
{
    public struct AdvertisementDto
    {
        public int? job_number { get; set; }
        public required string title { get; set; }
        public required string country { get; set; }
        public required string city { get; set; }
        public required string employeement_type { get; set; }
        public required string arrangement { get; set; }
        public required string experience { get; set; }
        public required DateTime posted_date { get; set; }
        public float? salary { get; set; }
        public string? currency_unit { get; set; }
        public DateTime? submission_deadline { get; set; }
        public string? job_description { get; set; }
        public string? company_name { get; set; }
        public string? company_logo_url { get; set; }
        public string? field_name { get; set; }
        public bool is_expired { get; set; }
        public ICollection<Skill> missingSkills { get; set; }
        public ICollection<Skill> matchingSkills { get; set; }
    }
}
