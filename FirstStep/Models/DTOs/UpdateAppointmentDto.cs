namespace FirstStep.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        public int appointment_id { get; set; }

        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }
}
