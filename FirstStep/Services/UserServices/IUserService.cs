using FirstStep.Models.DTOs;
using static FirstStep.Services.UserServices.UserService;

namespace FirstStep.Services.UserServices
{
    public interface IUserService
    {
        Task<AuthenticationResult> Authenticate(LoginRequestDto userObj);
        Task<string> RegisterUser(UserRegRequestDto userObj);

    }
}
