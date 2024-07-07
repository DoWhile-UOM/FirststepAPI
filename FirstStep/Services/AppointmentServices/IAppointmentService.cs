using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointment(AddAppointmentDto newAppointment);

        Task AssignToAdvertisement(int appointment_id, int advertisement_id);

        Task BookAppointment(int appointment_id, int seeker_id);

        Task<List<dailyInterviewDto>> GetSchedulesByDate(DateTime date);

        Task<bool> UpdateInterviewStatus(int appointment_id, Appointment.Status newStatus);

        Task<AppointmentAvailableDto> GetAvailabelSlots(int advertisment_id);

        Task<AppointmentDetailsDto> GetBookedAppointmentList(int advertisment_id);
    }
}
