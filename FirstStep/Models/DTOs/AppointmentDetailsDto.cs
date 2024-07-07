namespace FirstStep.Models.DTOs
{
    public class AppointmentDetailsDto
    {
        public IEnumerable<AppointmentDto>? BookedAppointments { get; set; }
        public IEnumerable<AppointmentDto>? FreeAppointments { get; set; }
    }
}
