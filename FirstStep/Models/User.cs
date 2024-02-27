using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [EmailAddress]
        public required string email { get; set; }

        public required string password_hash { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public string? user_type { get; set; }

        //public string token { get; set; }

        //public string Role { get; set; }
    }
}
