﻿using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;

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

        public async Task CreateAppointment(AddAppointmentDto newAppointment)
        {
            // check the company is exist
            var company = await _context.Companies.FindAsync(newAppointment.company_id);
            if (company == null)
            {
                throw new Exception("Company not found");
            }

            // check the advertisement is exist
            var advertisement = await _context.Advertisements.FindAsync(newAppointment.advertisement_id);
            if (advertisement == null)
            {
                throw new Exception("Advertisement not found");
            }
            if (advertisement.current_status != Advertisement.Status.interview.ToString())
            {
                throw new Exception("Advertisement is not in the interview, therefore can't add an appointment.");
            }

            var appointment = _mapper.Map<Appointment>(newAppointment);

            appointment.status = Appointment.Status.Pending.ToString();

            _context.Appointments.Add(appointment);
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
            var appointment = await FindById(appointment_id);

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

            appointment.status = Appointment.Status.Booked.ToString();

            await _context.SaveChangesAsync();
        }
    }
}