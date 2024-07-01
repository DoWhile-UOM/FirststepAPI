namespace FirstStep.Models.DTOs
{
    public class UpdateApplicationStatusDto
    {
        public required int application_id { get; set; }
        public bool is_called { get; set; } 
    }
}
