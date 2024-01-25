using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRManager : Employee
    {
        [JsonIgnore]
        public ICollection<Advertisement>? advertisements { get; set; }
    }
}
