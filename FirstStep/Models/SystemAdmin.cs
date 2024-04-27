using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class SystemAdmin : User
    {
        [JsonIgnore]
        public ICollection<Company>? verified_companies { get; set; }
    }
}
