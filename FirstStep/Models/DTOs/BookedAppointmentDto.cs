namespace FirstStep.Models.DTOs
{
    public class BookedAppointmentDto
    {
        public int user_id { get; set; }
        public int appointment_id { get; set; }
        public required DateTime start_time { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }
    }
}
