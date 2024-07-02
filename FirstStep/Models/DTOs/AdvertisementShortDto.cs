namespace FirstStep.Models.DTOs
{
    public struct AdvertisementShortDto
    {
        public int advertisement_id { get; set; }
        public string title { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string company_logo_url { get; set; }
        public string field_name { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string employeement_type { get; set; }
        public string arrangement { get; set; }
        public DateTime posted_date { get; set; }
        public bool is_saved { get; set; }
        public bool is_expired { get; set; }
        public bool can_apply { get; set; }
    }
}
