using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class OTPRequest
    {
        [Key]
        [EmailAddress]
        public required string email { get; set; }

        public required int otp { get; set; }


        [JsonIgnore]
        public DateTime expiry_date_time { get; set; }
    }
}
