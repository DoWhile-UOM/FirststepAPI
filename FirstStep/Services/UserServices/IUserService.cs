using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IUserService
    {
        Task<AuthenticationResult> Authenticate(LoginRequestDto userObj);

        Task<AuthenticationResult> RefreshToken(TokenApiDto tokenApiDto);

        Task<UpdateEmployeeDto> GetUserById(int user_id);

        Task UpdateUser(UpdateUserDto user);
    }
}
