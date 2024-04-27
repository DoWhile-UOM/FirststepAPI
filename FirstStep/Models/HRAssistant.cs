using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRAssistant : Employee
    {
        [JsonIgnore]
        public virtual ICollection<Application>? applications { get; set; }
    }
}
