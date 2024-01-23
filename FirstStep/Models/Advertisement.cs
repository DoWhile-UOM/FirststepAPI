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

        public required string location_province { get; set; }

        public required string location_city { get; set; }

        public required string employeement_type { get; set; }

        public required string arrangement { get; set; }

        public required bool is_experience_required { get; set; }

        public float salary { get; set; }

        public required DateTime posted_date { get; set; } = DateTime.Now;

        public DateTime submission_deadline { get; set; }

        public required string current_status { get; set; }

        public string? job_overview { get; set; }

        public string? job_responsibilities { get; set; }

        public string? job_qualifications { get; set; }

        public string? job_benefits { get; set; }

        public string? job_other_details { get; set; }


        [JsonIgnore]
        public HRManager? hrManager { get; set; }

        public required int hrManager_id { get; set; }        


        //[JsonIgnore]
        public virtual JobField? job_Field { get; set; }

        public required int field_id { get; set; }


        [JsonIgnore]
        public ICollection<Advertisement_Seeker>? advertisement_seekers { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Advertisement_ProfessionKeyword>? advertisement_professionKeywords { get; set; }
        //public List<ProfessionKeyword>? professionKeywords { get; set; }
    }
}
