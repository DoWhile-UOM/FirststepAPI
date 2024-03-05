using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class Application
    {
        [Key]
        public int application_Id { get; set; }

        public required string status { get; set; }

        public required DateTime submitted_date { get; set; } = DateTime.Now;
    }
}



