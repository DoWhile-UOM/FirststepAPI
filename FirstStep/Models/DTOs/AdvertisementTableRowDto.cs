namespace FirstStep.Models.DTOs
{
    public struct AdvertisementTableRowDto
    {
        public int advertisement_id { get; set; }
        public int job_number { get; set; }
        public string title { get; set; }
        public DateTime posted_date { get; set; }
        public string current_status { get; set; }
        public string field_name { get; set; }
        public bool has_permision_for_handling { get; set; }
        public int no_of_applications { get; set; }
        public int no_of_evaluated_applications { get; set; }
        public int no_of_accepted_applications { get; set; }
        public int no_of_rejected_applications { get; set; }
    }
}
