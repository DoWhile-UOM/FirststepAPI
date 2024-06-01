namespace FirstStep.Models.DTOs
{
    public class SeekerProfileViewDto
    {
        public required string email { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public int phone_number { get; set; }

        public required string bio { get; set; }

        public required string description { get; set; }

        public string? university { get; set; }

        public string? cVurl { get; set; } //nullable dla thibe

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }

        public required int field_id { get; set; }

        public int user_id { get; set; }

        //public virtual List<string>? seekerSkills { get; set; }
    }
}
