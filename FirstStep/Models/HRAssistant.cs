namespace FirstStep.Models
{
    public class HRAssistant : Employee
    {
        public virtual ICollection<Application>? applications { get; set; }
    }
}
