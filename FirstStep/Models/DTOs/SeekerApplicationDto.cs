namespace FirstStep.Models.DTOs
{
    public class SeekerApplicationDto
    {
        public required string email { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public int phone_number { get; set; }

        public string? linkedin { get; set; }

        public string? Cv { get; set; }

    }
}
