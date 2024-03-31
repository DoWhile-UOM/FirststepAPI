using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models.DTOs
{
    public class UserRegRequestDto
    {
        public required string email { get; set; }

        public required string password_hash { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

    }
}
