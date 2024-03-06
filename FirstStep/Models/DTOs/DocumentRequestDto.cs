namespace FirstStep.Models.DTOs
{
    public class DocumentRequestDto
    {
        
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
    }
}
