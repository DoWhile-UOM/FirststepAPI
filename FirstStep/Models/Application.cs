using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Application
    {
        [Key]
        public int application_Id { get; set; }

        public required string email { get; set; }

        public required string phone_number { get; set; }

        public required string status { get; set; }

        public required DateTime submitted_date { get; set; } = DateTime.Now;


        [JsonIgnore]
        public virtual Advertisement? advertisement { get; set; }

        public required int advertisement_id { get; set; }
    }
}