namespace FirstStep.Models.DTOs
{
    public class AddApplicationDto
    {
        public required string status { get; set; }

        public required DateTime submitted_date { get; set; } = DateTime.Now;

        public required int advertisement_id { get; set; }

        public required int user_id { get; set; }
    }
}
