using FirstStep.Models.DTOs;
using static FirstStep.Services.UserServices.UserService;

namespace FirstStep.Services.UserServices
{
    public interface IUserService
    {
        Task<AuthenticationResult> Authenticate(LoginRequestDto userObj);

        Task<AuthenticationResult> RefreshToken(TokenApiDto tokenApiDto);
        Task<string> RegisterUser(UserRegRequestDto userObj);


    }
}
