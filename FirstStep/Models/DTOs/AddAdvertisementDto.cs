namespace FirstStep.Models.DTOs
{
    public class AddAdvertisementDto
    {
        public int? job_number { get; set; }

        public string? title { get; set; }

        public string? location_province { get; set; }

        public string? location_city { get; set; }

        public string? employeement_type { get; set; }

        public string? arrangement { get; set; }

        public bool is_experience_required { get; set; }

        public float? salary { get; set; }

        public DateTime? submission_deadline { get; set; }

        public string? current_status { get; set; }

        public string? job_overview { get; set; }

        public string? job_responsibilities { get; set; }

        public string? job_qualifications { get; set; }

        public string? job_benefits { get; set; }

        public string? job_other_details { get; set; }

        public int? hrManager_id { get; set; }

        public int? field_id { get; set; }

        //public ICollection<int>? professionKeywordIds { get; set; }
    }
}
