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
                return Ok(await _appointmentService.GetBookedAppointmentList(advertismentId));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetSchedulesByDateAndCompany/{date}/Company/{companyId}")]
        public async Task<ActionResult<List<dailyInterviewDto>>> GetSchedulesByDate(DateTime date, int companyId)
        {
            var schedules = await _appointmentService.GetSchedulesByDate(date,companyId);
            return Ok(schedules);
        }

        [HttpGet]
        [Route("GetInterviewStat")]
        public async Task<IActionResult> GetInterviewStat(int companyId)
        {
            try
            {
                return Ok(await _appointmentService.GetInterviewStat(companyId));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPost]
        [Route("CreateAppointments")]
        public async Task<IActionResult> CreateAppointments(AddAppointmentDto newAppointment)
        {
            await _appointmentService.CreateAppointment(newAppointment);
            return Ok();
        }

        [HttpPatch]
        [Route("AssignToAdvertisement/appointment={appointment_id:int}/advertisement={advertisement_id:int}")]
        public async Task<IActionResult> AssignToAdvertisement(int appointment_id, int advertisement_id)
        {
            await _appointmentService.AssignToAdvertisement(appointment_id, advertisement_id);
            return Ok();
        }

        [HttpPatch]
        [Route("BookAppointment/{appointment_id:int}/{seeker_id:int}")]
        public async Task<IActionResult> BookAppointment(int appointment_id, int seeker_id)
        {
            await _appointmentService.BookAppointment(appointment_id, seeker_id);
            return Ok();
        }

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

        private ActionResult ReturnStatusCode(Exception e)
        {
            if (e is InvalidDataException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            else if (e is NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
