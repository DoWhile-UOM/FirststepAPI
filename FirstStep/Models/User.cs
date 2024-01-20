using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        public required string email { get; set; }

        public required string password { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public required string user_type { get; set; }
    }
}
