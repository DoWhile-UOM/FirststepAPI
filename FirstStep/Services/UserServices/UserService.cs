using FirstStep.Data;
using System.Text.RegularExpressions;
using System.Text;
using FirstStep.Models.DTOs;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;
using FirstStep.Helper;

namespace FirstStep.Services.UserServices
{
    public class UserService: IUserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }


        //Register User
        public async Task<string> RegisterUser(UserRegRequestDto userObj)
        {
            if (userObj == null)
                return "Null User";

            //check if email already exists
            if (await CheckEmailExist(userObj.email))
                return "Email Already exist";//email already exists


            //password strength check
            var passCheck = PasswordStrengthCheck(userObj.password_hash);

            if (!string.IsNullOrEmpty(passCheck))
                return passCheck.ToString();

            userObj.password_hash = PasswordHasher.Hasher(userObj.password_hash);//Hash password before saving to database
            //userObj.Role = "User";
            //userObj.token = CreateVerifyToken();

            User userNewObj = new User
            {
                email = userObj.email,
                password_hash = userObj.password_hash,
                first_name = userObj.first_name,
                last_name = userObj.last_name,
                user_type = "User",
                token = null,
                refresh_token = null
            };

            _context.Users.Add(userNewObj);
            _context.SaveChanges();

            return "User Registered Successfully";

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

        //Check if email already exists
        private async Task<bool> CheckEmailExist(string Email)
        {
            return await _context.Users.AnyAsync(x => x.email == Email);
        }












    }
}
