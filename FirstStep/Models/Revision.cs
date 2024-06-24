using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    [Index(nameof(status))]
    public class Revision
    {
        [Key]
        public int revision_id { get; set; }

        public string? comment { get; set; }

        public required DateTime date { get; set; } = DateTime.Now;

        [MaxLength(15)]
        public required string status { get; set; }


        [JsonIgnore]
        public virtual Application? application { get; set; }
        
        public required int application_id { get; set; }


        [JsonIgnore]
        public virtual Employee? employee { get; set; }
        
        public required int employee_id { get; set; }
    }
}
