using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;

namespace FirstStep.Services
{
    public interface IUserService
    {
        Task<AuthenticationResult> Authenticate(LoginRequestDto userObj);

        Task<AuthenticationResult> ResetPasswordRequest(string userEmail);

        Task ResetPassword(PasswordResetDto userObj);

        Task<AuthenticationResult> RefreshToken(TokenApiDto tokenApiDto);

        Task<UserDto> GetUserById(int user_id);

        Task UpdateUser(UpdateUserDto user);
    }
}
