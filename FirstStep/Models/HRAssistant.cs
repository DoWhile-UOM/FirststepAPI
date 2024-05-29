using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class HRAssistant : Employee
    {
        public virtual ICollection<Application>? applications { get; set; }

        public string comment { get; set; }

    }

}
