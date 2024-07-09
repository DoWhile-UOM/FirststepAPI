using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FirstStep.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AppointmentService(IEmailService emailService,DataContext dataContext, IMapper mapper) 
        {
            _context = dataContext;
            _mapper = mapper;
            _emailService = emailService;
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

            // send email to the seeker to book any advertisement with the message

            /*--------Email Sending Code Start--------*/
            var selectedApplicant = await _context.Applications
                .Include("seeker")
                .Where(a => a.advertisement_id == newAppointmentDto.advertisement_id && a.is_called == true && a.status==Application.ApplicationStatus.Accepted.ToString())
                .ToListAsync();

            foreach (var applicant in selectedApplicant)
            {
                await _emailService.SendEmailInterviewBook(applicant.seeker!.email, advertisement.title, company.company_name, applicant.seeker.user_id, advertisement.advertisement_id, newAppointmentDto.comment); //Send interview book Email
                //public async Task SendEmailInterviewBook(string email, string advertismentTitle, string company_name, int userid,int advertismentid)
            }

            /*--------Email Sending Code End--------*/

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
            var appointment = await _context.Appointments
                .Include(a => a.seeker)
                .Include(a=>a.advertisement)
                .Include(a => a.advertisement!.hrManager!.company)
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
            var seeker = await _context.Seekers
                .Include(a => a.appointments)
                .FirstOrDefaultAsync(a => a.user_id == seeker_id);

            if (seeker == null)
            {
                throw new Exception("Seeker not found");
            }

            if(appointment.seeker_id != null)
            {
                throw new Exception("Appointment is already booked");
            }

            //check whether seeker is already booked for the appointment
            if (seeker.appointments!.Any(a => a.advertisement_id == appointment.advertisement_id))
            {
                throw new Exception("Seeker is already booked an appoinment for the advertisement");
            }

            appointment.status = Appointment.Status.Booked.ToString();
            appointment.seeker_id = seeker_id;

            /*--------Email Sending Code Start--------*/

            string date = appointment.start_time.Date.ToString("yyyy-MM-dd");
            string time = appointment.start_time.TimeOfDay.ToString(@"hh\:mm");

            string advertismentTitle= appointment.advertisement!.title;
            string companyName =appointment.advertisement!.hrManager!.company!.company_name;

            await _emailService.SendEmailInterviewBookConfirm(seeker.email, advertismentTitle, companyName, date,time); //Send interview book Email

            /*--------Email Sending Code End--------*/

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
            if (appointment == null || 
                appointment.status == Appointment.Status.Missed.ToString() || 
                appointment.status == Appointment.Status.Complete.ToString())
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


            appointmentAvailable.slot = advertisement.appointments!.Where(a=>a.seeker_id==null && a.status==Appointment.Status.Pending.ToString()).Select(x => new AppointmentAvailabelTimeDto
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
                .Where(a => a.advertisement_id == advertisment_id && a.seeker_id != null && a.status == Appointment.Status.Booked.ToString())
                .ToListAsync();

            appointmentDetails.BookedAppointments = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            var freeApplication=await _context.Applications
                .Include("seeker")
                .Where(a => a.advertisement_id == advertisment_id && a.is_called == true)
                .ToListAsync();

            List <AppointmentDto> freeAppointmnets = new List<AppointmentDto>();

            foreach (var application in freeApplication)
            {
                if (appointments.Any(a => a.seeker_id == application.seeker_id))
                {
                    continue;
                }

                freeAppointmnets.Add(_mapper.Map<AppointmentDto>(application));
            }

            appointmentDetails.FreeAppointments = freeAppointmnets;

            return appointmentDetails;
        }

        public async Task CompleteInterviewSchedule(int advertisement_id)
        {
            // get all the booked appointments for the advertisement
            // email to each seeker about the interview schedule

            // delete all the empty appointments for the advertisement
            await DeleteEmptyAppointments(advertisement_id);
        }

        private async Task DeleteEmptyAppointments(int advertisement_id)
        {
            var appointments = await _context.Appointments
                .Where(a => a.advertisement_id == advertisement_id && a.seeker_id == null)
                .ToListAsync();

            _context.Appointments.RemoveRange(appointments);

            await _context.SaveChangesAsync();
        }
    }
}
