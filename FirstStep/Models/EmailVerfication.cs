using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class EmailVerfication  //Email address and OTP stores in here
    {
        [Key]
        public int id { get; set; }

        public required string email { get; set; }

        public required string otp { get; set; }

        public required string status { get; set; }

        public required DateTime otp_expiry { get; set; }

    }
}
