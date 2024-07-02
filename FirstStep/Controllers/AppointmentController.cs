using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("GetAvailabelSlots/{advertismentId:int}")]
        public async Task<IActionResult> GetAdvertisementByIdWithKeywords(int advertismentId)
        {
            try
            {
                return Ok(await _appointmentService.GetAvailabelSlots(advertismentId));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetBookedAppointmentList/{advertismentId:int}")]
        public async Task<IActionResult> GetBookedAppointmentList(int advertismentId)
        {
            try
            {
                return Ok(await _appointmentService.GetAvailabelSlots(advertismentId));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPost]
        [Route("CreateAppointments")]
        public async Task<IActionResult> CreateAppointments(AddAppointmentDto newAppointment)//Create Appointment Slot(Company Available Time)
        {
            await _appointmentService.CreateAppointment(newAppointment);
            return Ok();
        }

        [HttpPut]

        [HttpPatch]//Remove this cotroller only
        [Route("AssignToAdvertisement/appointment={appointment_id:int}/advertisement={advertisement_id:int}")]
        public async Task<IActionResult> AssignToAdvertisement(int appointment_id, int advertisement_id)
        {
            await _appointmentService.AssignToAdvertisement(appointment_id, advertisement_id);
            return Ok();
        }

        [HttpPatch]
        [Route("BookAppointment/appointment={appointment_id:int}/seeker={seeker_id:int}")]
        public async Task<IActionResult> BookAppointment(int appointment_id, int seeker_id)
        {
            await _appointmentService.BookAppointment(appointment_id, seeker_id);
            return Ok();
        }

        // Fetch schedules by date
        [HttpGet]
        [Route("GetByDate/{date}")]
        public async Task<ActionResult<List<dailyInterviewDto>>> GetSchedulesByDate(DateTime date)
        {
            var schedules = await _appointmentService.GetSchedulesByDate(date);
            return Ok(schedules);
        }


        // Update the status of an interview
        [HttpPatch]
        [Route("UpdateStatus/appointment={appointment_id:int}/status={newStatus}")]
        public async Task<IActionResult> UpdateInterviewStatus(int appointment_id, string newStatus)
        {
            // Validate the status value
            if (!Enum.TryParse<Appointment.Status>(newStatus, true, out var appointmentStatus))
            {
                return BadRequest("Invalid status value.");
            }

            var result = await _appointmentService.UpdateInterviewStatus(appointment_id, appointmentStatus);
            if (!result)
            {
                return BadRequest("Unable to update status or status change not allowed.");
            }

            return NoContent();
        }

    }
}
