namespace FirstStep.Models.DTOs
{
    public class PasswordResetDto
    {
        public required string? token { get; set; }
        public required string password { get; set; }
    }
}
