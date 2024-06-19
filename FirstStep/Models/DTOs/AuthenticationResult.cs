namespace FirstStep.Models.DTOs
{
    public class AuthenticationResult
    {
        public bool IsSuccessful { get; set; }
        public TokenApiDto? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
