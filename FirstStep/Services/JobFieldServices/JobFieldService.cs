﻿using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
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

        private async Task<JobField> GetById(int id)
        {
            JobField? jobField = await _context.JobFields.FindAsync(id);
            if (jobField is null)
            {
                throw new Exception("JobField not found.");
            }

            return jobField;
        }

        public async Task Create(JobField jobField)
        {
            jobField.field_id = 0;

            _context.JobFields.Add(jobField);
            await _context.SaveChangesAsync();
        }

        public async Task Update(JobField reqJobField)
        {
            JobField dbJobField = await GetById(reqJobField.field_id);

            dbJobField.field_name = reqJobField.field_name;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            JobField jobField = await GetById(id);

            _context.JobFields.Remove(jobField);
            await _context.SaveChangesAsync();
        }
    }
}
