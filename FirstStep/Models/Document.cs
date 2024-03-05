using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class Document
    {
        [Key]
        public int document_id { get; set; }
        public  required  string document_name { get; set; }
        public required string document_type { get; set; }
        public required byte[] document_size { get; set; }
        public required string document_extension { get; set; }
        public required string document_path { get; set; }
        public string? document_description { get; set; }

        /*public DateTime Date { get; set; } = DateTime.Now;*/

        /*public IFormFile formFile { get; set; }*/


    }
}
