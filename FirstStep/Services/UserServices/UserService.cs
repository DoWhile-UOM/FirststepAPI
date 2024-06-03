using FirstStep.Data;
using System.Text.RegularExpressions;
using System.Text;
using FirstStep.Models.DTOs;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;
using FirstStep.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;

namespace FirstStep.Services
{
    public class UserService: IUserService
    {
        private readonly DataContext _context;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserService(DataContext context, 
            ICompanyService companyService, 
            IEmployeeService employeeService, 
            IEmailService emailService, IMapper mapper)
        {
            _context = context;
            _companyService = companyService;
            _employeeService = employeeService;
            _emailService = emailService;
            _mapper = mapper;
        }

        //User Authentication
        public async Task<AuthenticationResult> Authenticate(LoginRequestDto userObj)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == userObj.email);
            if (user == null)
                return new AuthenticationResult { IsSuccessful = false, ErrorMessage = "Username Not Found" };

            if (!PasswordHasher.VerifyPassword(userObj.password, user.password_hash))

                return new AuthenticationResult { IsSuccessful = false, ErrorMessage = "Invalid Password" };


            user.token = await CreateJwt(user); //create access token for session
            var newAccessToken = user.token;
            var newRefreshToken = CreateRefreshToken(); //create refresh token
            user.refresh_token = newRefreshToken;
            await _context.SaveChangesAsync();// save changes to database

            return new AuthenticationResult { IsSuccessful = true, Token = new TokenApiDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken } };
        }

        //Refresh Token
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

        
        //Check if email already exists
        public async Task<bool> CheckEmailExist(string Email)
        {
            return await _context.Users.AnyAsync(x => x.email == Email);
        }

        ///Password Strength Checker
        private static string PasswordStrengthCheck(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }

        //Create JWT Token
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

        //Refresh token generator
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

        //Fetch user prinicipals from the expired access token
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
        //get user by id
        public async Task<UpdateEmployeeDto> GetUserById(int user_id)
        {
            User? user = await _context.Users.FindAsync(user_id);
            UpdateEmployeeDto employeeDto = _mapper.Map<UpdateEmployeeDto>(user);
            return employeeDto;
        }

        //update user by id
        public async Task UpdateUser(UpdateUserDto user)
        {
            var toBeUpdatedUser = await _context.Users.FindAsync(user.user_id);
            //validation
           if (toBeUpdatedUser is null)
            {
                throw new Exception("User doesn't exist");
            }
            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(user.password_hash);
            Console.WriteLine(passCheck);

            if(!string.IsNullOrEmpty(passCheck))
            {
                throw new Exception(passCheck);
            }

            //Hash password before saving
            user.password_hash = PasswordHasher.Hasher(user.password_hash);

            //saving to database
            ;

            toBeUpdatedUser.password_hash=user.password_hash;
            toBeUpdatedUser.first_name = user.first_name;
            toBeUpdatedUser.last_name = user.last_name;
            toBeUpdatedUser.email = user.email;

            await _context.SaveChangesAsync();
        }
    }
}
