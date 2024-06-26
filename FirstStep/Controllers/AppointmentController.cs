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
