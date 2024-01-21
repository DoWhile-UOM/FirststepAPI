using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services.JobFieldServices
{
    public class JobFieldService : IJobFieldService
    {
        private readonly DataContext _context;

        public JobFieldService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobField>> GetAll()
        {
            return await _context.JobFields.ToListAsync();
        }

        public async Task<JobField> GetById(int id)
        {
            JobField? jobField = await _context.JobFields.FindAsync(id);
            if (jobField is null)
            {
                throw new Exception("JobField not found.");
            }

            return jobField;
        }

        public async Task<JobField> Create(JobField jobField)
        {
            jobField.field_id = 0;

            _context.JobFields.Add(jobField);
            await _context.SaveChangesAsync();

            return jobField;
        }

        public async void Update(JobField jobField)
        {
            JobField dbJobField = await GetById(jobField.field_id);

            dbJobField.field_name = jobField.field_name;

            await _context.SaveChangesAsync();
        }

        public async void Delete(int id)
        {
            JobField jobField = await GetById(id);

            _context.JobFields.Remove(jobField);
            await _context.SaveChangesAsync();
        }
    }
}
