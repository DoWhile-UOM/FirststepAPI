using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRAssistant:Employee
    {
        [JsonIgnore]
        public ICollection<Advertisement>? advertisements { get; }

    }
}
