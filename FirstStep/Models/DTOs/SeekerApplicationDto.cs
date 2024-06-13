namespace FirstStep.Models.DTOs
{
    public struct SeekerApplicationDto
    {
        public required string email { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public int phone_number { get; set; }

        public required string cVurl { get; set; }

        public required string defualt_cv_url { get; set; }

        public string? linkedin { get; set; }

    }
}
