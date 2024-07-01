using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    [Index(nameof(email))]
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public required string email { get; set; }

        public required string password_hash { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public required string user_type { get; set; }

        public string? token { get; set; }
       
        public string? refresh_token { get; set; }

        public DateTime refresh_token_expiry { get; set; }

        public DateTime last_login_date { get; set; }

        public enum UserType { seeker, ca, hrm, hra, sa }
    }
}
