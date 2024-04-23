using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRManager : Employee
    {
        [JsonIgnore]
        public virtual Company? admin_company { get; set; }

        [JsonIgnore]
        public ICollection<Advertisement>? advertisements { get; set; }
    }
}
