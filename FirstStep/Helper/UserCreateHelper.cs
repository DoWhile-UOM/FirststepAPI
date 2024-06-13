using System.Text.RegularExpressions;
using System.Text;

namespace FirstStep.Helper
{
    public class UserCreateHelper
    {
        ///Password Strength Checker
        public static string PasswordStrengthCheck(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8");

            else if(!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric");

            else if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter");

            return sb.ToString();
        }
    }
}
