using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Advertisement
    {
        [Key]
        public required int advertisement_id { get; set; }

        public int? job_number { get; set; }

        public required string title { get; set; }

        public required string country { get; set; }

        public required string city { get; set; }

        public required string employeement_type { get; set; }

        public required string arrangement { get; set; }

        public required bool is_experience_required { get; set; }

        public float salary { get; set; }

        public required DateTime posted_date { get; set; } = DateTime.Now;

        public DateTime submission_deadline { get; set; }

        public required string current_status { get; set; }

        [MaxLength(2500)]
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
    }
}
