namespace FirstStep.Models.DTOs
{
    public struct AppliedAdvertisementShortDto
    {
        public int advertisement_id { get; set; }
        public string title { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string field_name { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string employeement_type { get; set; }
        public string arrangement { get; set; }
        public DateTime posted_date { get; set; }
        public string application_status { get; set; }
        public int application_id { get; set; }
    }
}
