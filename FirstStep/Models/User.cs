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

        public string? token { get; set; }

        public string? user_type { get; set; } //User role (Seeker ,Admin ,HRMgr ,HRAssnt, CmpAdmin )

        public string? refresh_token { get; set; }

        public DateTime refresh_token_expiry { get; set; }

        public string? organization { get; set; } //Organization of user for companies=company name, for seekers=seeker admin=Admin
    }
}
