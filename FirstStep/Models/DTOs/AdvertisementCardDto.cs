namespace FirstStep.Models.DTOs
{
    public struct AdvertisementCardDto
    {
        public int advertisement_id { get; set; }
        public string title { get; set; }
        public string company_name { get; set; }
        public string field_name { get; set; }
        public string location_province { get; set; }
        public string location_city { get; set; }
        public string employeement_type { get; set; }
        public string arrangement { get; set; }
        public DateTime posted_date { get; set; }
        public bool is_saved { get; set; }
    }
}
