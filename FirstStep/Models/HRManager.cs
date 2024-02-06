using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRManager : Employee
    {
        [JsonIgnore]
        public virtual Company? admin_company { get; set; } // for one to one relationship among Company Admin and Company

        [JsonIgnore]
        public ICollection<Advertisement>? advertisements { get; set; }
    }
}
