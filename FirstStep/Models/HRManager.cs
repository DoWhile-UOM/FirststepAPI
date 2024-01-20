using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRManager : Employee
    {
        [JsonIgnore]
        public required ICollection<Advertisement> advertisements { get; set; }
    }
}
