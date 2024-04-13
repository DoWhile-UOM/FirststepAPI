using FirstStep.Models.DTOs;

namespace FirstStep.Services.UserServices
{
    public interface IUserService
    {
        Task<string> RegisterUser(UserRegRequestDto userObj);

    }
}
