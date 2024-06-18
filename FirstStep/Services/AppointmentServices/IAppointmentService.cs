using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointment(AddAppointmentDto newAppointment);

        Task AssignToAdvertisement(int appointment_id, int advertisement_id);

        Task BookAppointment(int appointment_id, int seeker_id);

        Task DummyService(int? test);
    }
}
