using Microsoft.AspNetCore.Authorization;

namespace FirstStep.AuthHandler
{
    public class AuthRequirement: IAuthorizationRequirement
    {
        public string ExpectedUsername { get; }
        public AuthRequirement(string expectedUsername)
        {
            ExpectedUsername = expectedUsername;

        }
    }
}
