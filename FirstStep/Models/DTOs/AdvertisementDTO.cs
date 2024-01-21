namespace FirstStep.Models.DTOs
{
    public class AdvertisementDto
    {
        public required string advertisement_title { get; set; }
        public required string advertisement_description { get; set; }
        public required string advertisement_location { get; set; }
        public required string advertisement_salary { get; set; }
        public required string advertisement_contact { get; set; }
        public required string advertisement_email { get; set; }
        public required string advertisement_website { get; set; }
        public required string advertisement_image { get; set; }
        public required string advertisement_video { get; set; }
        public required string advertisement_date { get; set; }
        public required List<string> professionKeywords { get; set; }
        public required string field_name { get; set; }
    }
}
