using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly DataContext _context;

        public AdvertisementService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Advertisement>> GetAll()
        {
            return await _context.Advertisements.ToListAsync();
        }

        public async Task<Advertisement> GetById(int id)
        {
            Advertisement? advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement is null)
            {
                throw new Exception("Advertisement not found.");
            }

            return advertisement;
        }

        public async Task<Advertisement> Create(Advertisement advertisement)
        {
            advertisement.advertisement_id = 0;

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            return advertisement;
        }

        public async void Update(Advertisement advertisement)
        {
            Advertisement dbAdvertisement = await GetById(advertisement.advertisement_id);

            dbAdvertisement.job_number = advertisement.job_number;
            dbAdvertisement.title = advertisement.title;
            dbAdvertisement.location_province = advertisement.location_province;
            dbAdvertisement.location_city = advertisement.location_city;
            dbAdvertisement.employeement_type = advertisement.employeement_type;
            dbAdvertisement.arrangement = advertisement.arrangement;
            dbAdvertisement.is_experience_required = advertisement.is_experience_required;
            dbAdvertisement.salary = advertisement.salary;
            dbAdvertisement.posted_date = advertisement.posted_date;
            dbAdvertisement.submission_deadline = advertisement.submission_deadline;
            dbAdvertisement.current_status = advertisement.current_status;
            dbAdvertisement.job_overview = advertisement.job_overview;
            dbAdvertisement.job_responsibilities = advertisement.job_responsibilities;
            dbAdvertisement.job_qualifications = advertisement.job_qualifications;
            dbAdvertisement.job_benefits = advertisement.job_benefits;
            dbAdvertisement.job_other_details = advertisement.job_other_details;
            dbAdvertisement.hrManager_id = advertisement.hrManager_id;
            dbAdvertisement.field_id = advertisement.field_id;
        }

        public async void Delete(int id)
        {
            Advertisement advertisement = await GetById(id);
            
            _context.Advertisements.Remove(advertisement);
            _context.SaveChanges();
        }

        // for find no of applications for a job
        /*
        public Task<int> findNoOfApplications(int job_id)
        {
            var dbAdvertisement = findAdvertisement(job_id);
            if (dbAdvertisement == null)
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
            //return dbAdvertisement.no_of_applications;
        }*/
    }
}
