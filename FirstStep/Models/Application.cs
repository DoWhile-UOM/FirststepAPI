using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Application
    {
        [Key]
        public int application_Id { get; set; }

        public required string status { get; set; }

        public required DateTime submitted_date { get; set; } = DateTime.Now;


        [JsonIgnore]
        public virtual Advertisement? advertisement { get; set; }//one to many

        public required int advertisement_id { get; set; }


        [JsonIgnore]
        public virtual Seeker? seeker { get; set; }//one to many

        public required int user_id { get; set; }
    }
}