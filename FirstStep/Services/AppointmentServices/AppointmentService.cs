using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AppointmentService(DataContext dataContext, IMapper mapper) 
        {
            _context = dataContext;
            _mapper = mapper;
        }

        private async Task<Appointment> FindById(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }
            
            return appointment;
        }

        public async Task CreateAppointment(AddAppointmentDto newAppointmentDto)
        {
            // check the company is exist
            var company = await _context.Companies.FindAsync(newAppointmentDto.company_id);
            if (company == null)
            {
                throw new Exception("Company not found");
            }

            // check the advertisement is exist
            var advertisement = await _context.Advertisements
                .Include("appointments")
                .Where(a => a.advertisement_id == newAppointmentDto.advertisement_id)
                .FirstOrDefaultAsync();

            if (advertisement == null)
            {
                throw new Exception("Advertisement not found");
            }
            if (advertisement.current_status != Advertisement.Status.hold.ToString())
            {
                throw new Exception("Advertisement is not in the hold, therefore can't add an appointment.");
            }

            // delete all appointments for the advertisement
            advertisement.appointments!.Clear();

            // change advertisement status to interview
            advertisement.current_status = Advertisement.Status.interview.ToString();
            advertisement.interview_duration = newAppointmentDto.duration;

            foreach (var time_slot in newAppointmentDto.time_slots)
            {
                Appointment newAppointment = new Appointment()
                {
                    advertisement_id = newAppointmentDto.advertisement_id,
                    company_id = newAppointmentDto.company_id,
                    start_time = time_slot,
                    status = Appointment.Status.Pending.ToString()
                };

                await _context.Appointments.AddAsync(newAppointment);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(UpdateAppointmentDto slot)
        {
            /* check the company is exist
            var company = await _context.Companies.FindAsync(slot.company_id);
            if (company == null)
            {
                throw new Exception("Company not found");
            }

            // check the advertisement is exist
            var advertisement = await _context.Advertisements.FindAsync(slot.advertisement_id);
            if (advertisement == null)
            {
                throw new Exception("Advertisement not found");
            }
            if (advertisement.current_status != Advertisement.Status.interview.ToString())
            {
                throw new Exception("Advertisement is not in the interview, therefore can't Update an appointment.");
            }*/

            var appointment = await _context.Appointments.FindAsync(slot.appointment_id);

            //appointment.start_time = slot.start_time;
            //appointment.end_time = slot.end_time;

            //_context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();
        }

        public async Task AssignToAdvertisement(int appointment_id, int advertisement_id)
        {
            var appointment = await FindById(appointment_id);

            var advertisement = await _context.Advertisements.FindAsync(advertisement_id);
            if (advertisement == null)
            {
                throw new Exception("Advertisement not found");
            }

            if (advertisement.current_status != Advertisement.Status.interview.ToString())
            {
                throw new Exception("Advertisement is not in the interview, therefore can't add an appointment.");
            }

            appointment.advertisement_id = advertisement_id;
            appointment.advertisement = advertisement;

            await _context.SaveChangesAsync();
        }

        public async Task BookAppointment(int appointment_id, int seeker_id)
        {
            var appointment = await _context.Appointments.
                Include(a => a.seeker)
                .Include(a=>a.seeker!.appointments)
                .FirstOrDefaultAsync(a => a.appointment_id == appointment_id);

            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }


            // check the appointment is assigned to an advertisement
            if (appointment.advertisement_id == null)
            {
                throw new Exception("Appointment is not assigned to an advertisement");
            }

            // check the seeker is exist
            var seeker = await _context.Seekers.FindAsync(seeker_id);
            if (seeker == null)
            {
                throw new Exception("Seeker not found");
            }
            if(appointment.seeker_id!=null)
            {
                throw new Exception("Appointment is already booked");
            }

            //check whether seeker is already booked for the appointment
            if (appointment.seeker!.appointments!.Any(a => a.advertisement_id == appointment.advertisement_id))
            {
                throw new Exception("Seeker is already booked for the appointment");
            }

            appointment.status = Appointment.Status.Booked.ToString();
            appointment.seeker_id = seeker_id;

            await _context.SaveChangesAsync();
        }

        public async Task<List<dailyInterviewDto>> GetSchedulesByDate(DateTime date)
        {
            return await _context.Appointments
                .Include(a => a.advertisement)
                .Include(a => a.seeker)
                .Where(a => a.start_time.Date == date.Date)
                .Select(a => new dailyInterviewDto
                {
                    appointment_id = a.appointment_id,
                    status = Enum.Parse<Appointment.Status>(a.status, true), // Parse with case-insensitivity
                    start_time = a.start_time,
                    end_time = a.start_time.AddMinutes(a.advertisement!.interview_duration), 
                    title = a.advertisement.title,
                    first_name = a.seeker != null ? a.seeker.first_name : "N/A",
                    last_name = a.seeker != null ? a.seeker.last_name : "N/A"  
                }).ToListAsync();
        }


        public async Task<bool> UpdateInterviewStatus(int appointment_id, Appointment.Status newStatus)
        {
            var appointment = await FindById(appointment_id);
            // Check if the appointment can be updated
            if (appointment == null || appointment.status == Appointment.Status.Missed.ToString() || appointment.status == Appointment.Status.Complete.ToString())
            {
                return false;
            }

            appointment.status = newStatus.ToString();
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AppointmentAvailableDto> GetAvailabelSlots(int advertisment_id)
        {
            Advertisement? advertisement = await _context.Advertisements
                .Include(x => x.appointments)
                .Include(x => x.hrManager!.company)
                .FirstOrDefaultAsync(x => x.advertisement_id == advertisment_id);

            if (advertisement == null)
            {
                throw new Exception("Advertisement not found");
            }

            if (advertisement.current_status != Advertisement.Status.interview.ToString())
            {
                throw new Exception("Advertisement is not in the interview, therefore can't add an appointment.");
            }

            if (advertisement.appointments == null)
            {
                throw new Exception("No appointments found");
            }

            AppointmentAvailableDto appointmentAvailable = new AppointmentAvailableDto();

            appointmentAvailable.slot = advertisement.appointments!.Select(x => new AppointmentAvailabelTimeDto
            {
                appointment_id = x.appointment_id,
                start_time = x.start_time
            }).ToList();

            appointmentAvailable.title = advertisement.title;
            appointmentAvailable.interview_duration = advertisement.interview_duration;
            appointmentAvailable.company_name = advertisement.hrManager!.company!.company_name;

            return appointmentAvailable;
        }


        public async Task<AppointmentDetailsDto> GetBookedAppointmentList(int advertisment_id)
        {
            AppointmentDetailsDto appointmentDetails = new AppointmentDetailsDto();

            //get the appointments for the advertisement id
            var appointments = await _context.Appointments
                .Include("seeker")
                .Where(a => a.advertisement_id == advertisment_id && a.seeker_id!=null && a.status==Appointment.Status.Booked.ToString())
                .ToListAsync();

            appointmentDetails.BookedAppointments = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            var freeApplication=await _context.Applications
                .Include("seeker")
                .Where(a => a.advertisement_id == advertisment_id && a.is_called==true)
                .ToListAsync();

            List <AppointmentDto> freeAppointmnets=new List<AppointmentDto>();

            foreach (var application in freeApplication)
            {
                if(appointments.Any(a=>a.seeker_id==application.seeker_id))
                {
                    continue;
                }
                freeAppointmnets.Add(_mapper.Map<AppointmentDto>(application));
            }

            appointmentDetails.FreeAppointments = freeAppointmnets;


            return appointmentDetails;
        }





    }
}
