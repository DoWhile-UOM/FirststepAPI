using AutoMapper;
using FirstStep.Data;
using FirstStep.Helper;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FirstStep.Services
{
    public class UserService: IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        
        private readonly Dictionary<string, int> _passwordResetTokens = new Dictionary<string, int>();
        private static readonly Random random = new Random();

        public UserService(DataContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
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
        
        public async Task<AuthenticationResult> ResetPasswordRequest(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == userEmail);
            if (user == null)
                throw new Exception("User not found.");

            string token=GenerateRandomString(10);

            _passwordResetTokens.Add(token,user.user_id);


            //Call Email service to send reset password email
            var result = await _emailService.CARegIsSuccessfull(user.email, token, "test");
            Console.WriteLine(token);

            return new AuthenticationResult { IsSuccessful = true};
        }

        private static string GenerateRandomString(int length)
        {
            const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(charset, length)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<AuthenticationResult> ResetPassword(PasswordResetDto userObj)
        {
            if (userObj.token == null)
            {
                throw new Exception("Token is null.");
            }

            if (_passwordResetTokens.TryGetValue(userObj.token, out var userId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.user_id == userId);

                if (user == null)
                    throw new Exception("Invalid Request.");

                //password strength check
                var passCheck = UserCreateHelper.PasswordStrengthCheck(userObj.password);

                if (!string.IsNullOrEmpty(passCheck))
                {
                    throw new Exception(passCheck.ToString());
                }

                user.password_hash = PasswordHasher.Hasher(userObj.password);//Hash password before saving to database
                _passwordResetTokens.Remove(userObj.token);

                await _context.SaveChangesAsync();

                return new AuthenticationResult { IsSuccessful = true };
            }
            else
            {
                throw new Exception("Invalid Token.");
            }
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

        public async Task<UserDto> GetUserById(int user_id)
        {
            User? user = await _context.Users.FindAsync(user_id);
            UserDto userDto = _mapper.Map<UserDto>(user);
            return userDto;
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

        private async Task<List<ActiveUsers>> GetActiveUsersAsync()
        {
            var activeUsers = await _context.Users
                .Where(user => user.last_login_date <= DateTime.Now.AddDays(-30))
                .ToListAsync();

            return _mapper.Map<List<ActiveUsers>>(activeUsers);
        }

        private async Task<List<ActiveUsers>> GetInactiveUsersAsync()
        {
            var inactiveUsers = await _context.Users
                .Where(user => user.last_login_date > DateTime.Now.AddDays(-90))
                .ToListAsync();

            return _mapper.Map<List<ActiveUsers>>(inactiveUsers);
        }

        public async Task<LoggingsDto> GetLoggingsOfUsersAsync()
        {
            List<ActiveUsers> activeUsers = await GetActiveUsersAsync();
            List<ActiveUsers> inactiveUsers = await GetInactiveUsersAsync();

            int tot_active = activeUsers.Count() - (activeUsers.Count(user => user.user_type == "sa"));
            int tot_inactive = inactiveUsers.Count() - (inactiveUsers.Count(user => user.user_type == "sa"));

            int tot_cmpny_active_users = tot_active - activeUsers.Count(user => user.user_type == "seeker");
            int tot_cmpny_inactive_users = tot_inactive - inactiveUsers.Count(user => user.user_type == "seeker");

            var loggingsDto = new LoggingsDto
            {
                activeTot = tot_active,
                inactiveTot = tot_inactive,
                activeCA = activeUsers.Count(user => user.user_type == "ca"),
                inactiveCA = inactiveUsers.Count(user => user.user_type == "ca"),
                activeHRM = activeUsers.Count(user => user.user_type == "hrm"),
                inactiveHRM = inactiveUsers.Count(user => user.user_type == "hrm"),
                activeHRA = activeUsers.Count(user => user.user_type == "hra"),
                inactiveHRA = inactiveUsers.Count(user => user.user_type == "hra"),
                activeSeeker = activeUsers.Count(user => user.user_type == "seeker"),
                inactiveSeeker = inactiveUsers.Count(user => user.user_type == "seeker"),
                activeCmpUsers = tot_cmpny_active_users,
                inactiveCmpUsers = tot_cmpny_inactive_users
            };
            return loggingsDto;
        }
    }
}
