using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Appointment
    {
        [Key]
        public int appointment_id { get; set; }

        public required string status { get; set; }

        public required DateTime start_time { get; set; }


        [JsonIgnore]
        public virtual Company? company { get; set; }

        public required int company_id { get; set; }


        [JsonIgnore]
        public virtual Advertisement? advertisement { get; set; }

        public int? advertisement_id { get; set; }


        [JsonIgnore]
        public virtual Seeker? seeker { get; set; }

        public int? seeker_id { get; set; }


        public enum Status { Free, Pending, Booked, Missed, Complete }
    }
}