using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using FirstStep.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FirstStep.Models.ServiceModels;

namespace FirstStep.Services
{
    public class UserService: IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AuthenticationResult> Authenticate(LoginRequestDto userObj)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == userObj.email);
            
            if (user == null)
            {
                return new AuthenticationResult { 
                    IsSuccessful = false, 
                    ErrorMessage = "Username Not Found" 
                };
            }
            
            if (!PasswordHasher.VerifyPassword(userObj.password, user.password_hash))
            {
                return new AuthenticationResult 
                { 
                    IsSuccessful = false, 
                    ErrorMessage = "Invalid Password" 
                };
            }  

            var newAccessToken = await CreateJwt(user); //create access token for session
            var newRefreshToken = CreateRefreshToken(); //create refresh token
            
            user.refresh_token = newRefreshToken;
            user.token = newAccessToken;
            user.last_login_date = DateTime.Now;

            await _context.SaveChangesAsync();// save changes to database

            return new AuthenticationResult 
            { 
                IsSuccessful = true, 
                Token = new TokenApiDto 
                { 
                    AccessToken = newAccessToken, 
                    RefreshToken = newRefreshToken 
                } 
            };
        }

        public async Task<AuthenticationResult> RefreshToken(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return new AuthenticationResult { IsSuccessful = false, ErrorMessage = "Input is Null" };
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity?.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == username);
            if (user is null || user.refresh_token != refreshToken || user.refresh_token_expiry <= DateTime.Now)
                return new AuthenticationResult { IsSuccessful = false, ErrorMessage = "Invalid Request" };

            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.refresh_token = newRefreshToken;
            await _context.SaveChangesAsync();

            return new AuthenticationResult { IsSuccessful = true, Token = new TokenApiDto { AccessToken = await newAccessToken, RefreshToken = newRefreshToken } };
        }

        private async Task<string> CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("AshanMatheeshaSecretKeythisisSecret");

            var identity = new ClaimsIdentity();

            if (user.user_type == User.UserType.ca.ToString() || user.user_type == User.UserType.hrm.ToString() || user.user_type == User.UserType.hra.ToString())
            {
                Employee? emp = await _context.Employees.Include("company").Where(x => x.user_id == user.user_id).FirstOrDefaultAsync();

                if (emp == null)
                    return "Employee Not Found";
                if (emp.company == null)
                    return "Company Not Found";

                identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, emp.user_id.ToString()),
                    new Claim(ClaimTypes.Role, emp.user_type), //Store role in JWT Token (seeker ,sa ,HRM ,HRA, ca)
                    new Claim(ClaimTypes.GivenName, emp.first_name + ' ' + emp.last_name),
                    new Claim("CompanyName", emp.company.company_name),
                    new Claim("CompanyID", emp.company.company_id.ToString()),
                });
            }
            else
            {
                identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                    new Claim(ClaimTypes.Role, user.user_type), //Store role in JWT Token (seeker ,sa ,HRM ,HRA, ca)
                    new Claim(ClaimTypes.GivenName, user.first_name + ' ' + user.last_name)
                });
            }

            identity.AddClaim(new Claim(ClaimTypes.Webpage, user.user_type));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _context.Users
                .Any(a => a.refresh_token == refreshToken);
            if (tokenInUser)//If refresh token exist generate new one otherwise return
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("AshanMatheeshaSecretKeythisisSecret");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;
        }

        public async Task<UpdateEmployeeDto> GetUserById(int user_id)
        {
            User? user = await _context.Users.FindAsync(user_id);
            UpdateEmployeeDto employeeDto = _mapper.Map<UpdateEmployeeDto>(user);
            return employeeDto;
        }

        public async Task UpdateUser(UpdateUserDto user)
        {
            var toBeUpdatedUser = await _context.Users.FindAsync(user.user_id);
            
            if (toBeUpdatedUser is null)
            {
                throw new Exception("User doesn't exist");
            }

            if(user.password_hash.Length != 0)
            {
                //password strength check
                var passCheck = UserCreateHelper.PasswordStrengthCheck(user.password_hash);
                Console.WriteLine(passCheck);

                if (!string.IsNullOrEmpty(passCheck))
                {
                    throw new Exception(passCheck);
                }

                //Hash password before saving
                user.password_hash = PasswordHasher.Hasher(user.password_hash);

                //saving
                toBeUpdatedUser.password_hash = user.password_hash;
            }
            //saving data in the database
            toBeUpdatedUser.first_name = user.first_name;
            toBeUpdatedUser.last_name = user.last_name;
            toBeUpdatedUser.email = user.email;

            await _context.SaveChangesAsync();
        }
    }
}
