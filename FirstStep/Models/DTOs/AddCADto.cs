namespace FirstStep.Models.DTOs
{
    public class AddCADto
    {
        public required string email { get; set; }

        public required string password { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

    }
}
