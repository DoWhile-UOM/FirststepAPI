using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Advertisement
    {   
        [Key]
        public required int advertisement_id { get; set; }

        public int? job_number { get; set; }

        [MaxLength(100)]
        public required string title { get; set; }

        [MaxLength(40)]
        public required string country { get; set; }

        [MaxLength(40)]
        public required string city { get; set; }

        [MaxLength(15)]
        public required string employeement_type { get; set; }

        [MaxLength(15)]
        public required string arrangement { get; set; }

        [MaxLength(15)]
        public required string experience { get; set; }

        public float? salary { get; set; }

        [MaxLength(5)]
        public string? currency_unit { get; set; }

        public required DateTime posted_date { get; set; } = DateTime.Now;

        public DateTime? submission_deadline { get; set; }

        public DateTime? expired_date { get; set; }

        [MaxLength(10)]
        public required string current_status { get; set; }

        [MaxLength(4000)]
        public string? job_description { get; set; }


        [JsonIgnore]
        public virtual HRManager? hrManager { get; set; }

        public required int hrManager_id { get; set; }


        [JsonIgnore]
        public virtual JobField? job_Field { get; set; }

        public required int field_id { get; set; }


        [JsonIgnore]
        public virtual ICollection<Application>? applications { get; set; }

        [JsonIgnore]
        public virtual ICollection<Seeker>? savedSeekers { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfessionKeyword>? professionKeywords { get; set; }

        [JsonIgnore]
        public virtual ICollection<Skill>? skills { get; set; }


        public enum Status { active, hold, closed, interview }
    }
}
