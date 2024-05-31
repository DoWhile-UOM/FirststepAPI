namespace FirstStep.Models.DTOs
{
    public class SeekerApplicationViewDto
    {
        public required string email { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public int phone_number { get; set; }

        public required string bio { get; set; }

        public required string cVurl { get; set; }

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }
    }
}
