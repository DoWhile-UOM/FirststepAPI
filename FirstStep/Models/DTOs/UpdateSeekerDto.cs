using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models.DTOs
{
    public class UpdateSeekerDto
    {
        [Required]
        public string email { get; set; }

        public string? password { get; set; }

        [Required]
        public string first_name { get; set; }

        [Required]
        public string last_name { get; set; }

        //[Required, DataType(DataType.PhoneNumber)]
        [Required]  
        public int phone_number { get; set; }

        [Required]
        public string bio { get; set; }

        [Required]
        public string description { get; set; }

        public string? university { get; set; }

        public string CVurl { get; set; }

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }

        [Required]
        public int field_id { get; set; }

        public List<string>? seekerSkills { get; set; }

        public IFormFile? cvFile { get; set; }

        public IFormFile? profilePictureFile { get; set; } 


    }
}
