using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class OTPRequests
    {
        [Key]
        [EmailAddress]
        public required string email { get; set; }

        public required int otp { get; set; }
    }
}
