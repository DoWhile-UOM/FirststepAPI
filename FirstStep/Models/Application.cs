using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    [Index(nameof(status))]
    public class Application
    {
        [Key]
        public int application_Id { get; set; }

        [MaxLength(15)]
        public required string status { get; set; }

        public required DateTime submitted_date { get; set; } = DateTime.Now;

        public required string CVurl { get; set; }

        public bool is_called { get; set; } = false;//is checked->true, is not checked->false


        [JsonIgnore]
        public virtual Advertisement? advertisement { get; set; }

        public required int advertisement_id { get; set; }


        [JsonIgnore]
        public virtual Seeker? seeker { get; set; }

        public required int seeker_id { get; set; }


        [JsonIgnore]
        public virtual HRAssistant? assigned_hrAssistant { get; set; }
        
        public int? assigned_hrAssistant_id { get; set; }


        [JsonIgnore]
        public virtual ICollection<Revision>? revisions { get; set; }


        public enum ApplicationStatus { Pass, NotEvaluated, Accepted, Rejected, Done }
    }
}