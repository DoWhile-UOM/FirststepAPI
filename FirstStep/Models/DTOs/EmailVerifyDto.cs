namespace FirstStep.Models.DTOs
{
    public class EmailVerifyDto
    {
        public required string email { get; set; }

        public required string otp { get; set; }
    }
}
