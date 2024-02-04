namespace FirstStep.Models
{
    public class SystemAdmin : User
    {
        public ICollection<RegisteredCompany>? registeredCompanies { get; set; }
    }
}
