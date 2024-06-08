namespace FirstStep.Models.DTOs
{
    public struct ApplicationStatusDto
    {
        public required string status { get; set; }
        public string? cv_name { get; set; }
        public required DateTime submitted_date { get; set; }
        public required int advertisement_id { get; set; }
        public required int seeker_id { get; set; }




    }
}
