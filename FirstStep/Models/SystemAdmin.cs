using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class SystemAdmin : User
    {
        [JsonIgnore]
        public ICollection<RegisteredCompany>? registeredCompanies { get; set; }
    }
}
