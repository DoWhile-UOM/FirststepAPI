namespace FirstStep.Models.DTOs
{
    public class dailyInterviewDto
    {
        public int appointment_id { get; set; }

        public required Appointment.Status status { get; set; }  // Using enum type

        public required DateTime start_time { get; set; }

        public required DateTime end_time { get; set; }

        public required string title { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }
    }  

}
