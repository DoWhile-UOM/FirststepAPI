namespace FirstStep.Models.DTOs
{
    public struct AdvertisementHRATableRowDto
    {
        public int advertisement_id { get; set; }
        public int job_number { get; set; }
        public string title { get; set; }
        public string field_name { get; set; }
        public int no_of_applications { get; set; }
        public int no_of_assigned_applications { get; set; }
        public int no_of_evaluated_applications { get; set; }
        public int no_of_nonevaluated_applications { get; set; }
    }
}
