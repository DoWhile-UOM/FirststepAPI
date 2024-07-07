namespace FirstStep.Models.DTOs
{
    public class AppointmentDto
    {
        public int seeker_id { get; set; }
        public required string seeker_name { get; set; }

        public DateTime? start_time { get; set; }

    }
}
