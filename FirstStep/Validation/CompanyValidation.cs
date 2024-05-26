using FirstStep.Models;

namespace FirstStep.Validation
{
    public class CompanyValidation
    {
        public static bool IsRegistered(Company company)
        {
            return company.verification_status;
        }
    }
}
