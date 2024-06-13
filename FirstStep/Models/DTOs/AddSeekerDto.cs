namespace FirstStep.Models.DTOs
{
    public class AddSeekerDto
    {
        public required string first_name { get; set; }

        public  required string last_name { get; set; }

        public required string password { get; set; }

        public required int phone_number { get; set; }

        public required string email { get; set; }

        public string? university { get; set; }

        public string? linkedin { get; set; }

        public required string bio { get; set; }

        public required int field_id { get; set; }

        public string? cVurl { get; set; }

        public string? profile_picture { get; set; }

        public string description { get; set; }

        public virtual List<string>? seekerSkills { get; set; }

        public IFormFile? cvFile { get; set; } // Added for CV file upload

        public IFormFile? profilePictureFile { get; set; } // Added for profile picture file upload


    }
}
