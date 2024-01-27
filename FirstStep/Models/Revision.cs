using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
{
    public class Revision
    {
        [Key]
        public int revision_id { get; set; }

        public string? comment { get; set; }

        public required DateTime date { get; set; } = DateTime.Now;

        public required string status { get; set; }
    }
}
