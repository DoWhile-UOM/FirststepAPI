using FirstStep.Models.DTOs;

namespace FirstStep.Models.ServiceModels
{
    public class AuthenticationResult
    {
        public bool IsSuccessful { get; set; }
        public TokenApiDto? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
