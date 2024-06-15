using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class RevisionService : IRevisionService
    {
        private readonly DataContext _context;

        public RevisionService(DataContext context)
        {
            _context = context;
        }

        enum ApplicationStatus { Evaluated, NotEvaluated, Accepted, Rejected ,Passed }

        public async Task<IEnumerable<Revision>> GetAll()
        {
            return await _context.Revisions.ToListAsync();
        }

        public async Task<Revision> GetById(int id)
        {
            Revision? revision = await _context.Revisions.FindAsync(id);
            if (revision is null)
            {
                throw new Exception("Revision not found.");
            }

            return revision;
        }

        public async Task<List<Revision>> GetByApplicationID(int applicationID)
        {
            return await _context.Revisions
                .Include("application")
                .Include("employee")
                .Where(r => r.application_id == applicationID)
                .ToListAsync();
        }

        public async Task<string> GetCurrentStatus(int applicationID)
        {
            Revision? last_revision = await _context.Revisions
                .Where(r => r.application_id == applicationID)
                .OrderByDescending(r => r.date)
                .FirstOrDefaultAsync();

            if (last_revision is null)
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            return last_revision.status;
        }

        public async Task<Revision?> GetLastRevision(int applicationID)
        {
            var lastRevision = await _context.Revisions
                .Include("employee")
                .Where(r => r.application_id == applicationID)
                .OrderByDescending(r => r.date)
                .FirstOrDefaultAsync();

            return lastRevision;
        }

        public string GetCurrentStatus(Application application)
        {
            if (application.revisions is null)
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            Revision? last_revision = application.revisions.OrderByDescending(r => r.date).FirstOrDefault();

            if (last_revision is null)
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            return last_revision.status;
        }

        public async Task Create(Revision revision)
        {
            revision.revision_id = 0;

            _context.Revisions.Add(revision);
            await _context.SaveChangesAsync();

            // Update application status
            var application = await _context.Applications.FindAsync(revision.application_id);
            if (application != null)
            {
                application.status = revision.status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Revision revision)
        {
            Revision dbRevision = await GetById(revision.revision_id);

            dbRevision.comment = revision.comment;
            dbRevision.date = revision.date;
            dbRevision.status = revision.status;

            await _context.SaveChangesAsync();

            // Update application status
            var application = await _context.Applications.FindAsync(revision.application_id);
            if (application != null)
            {
                application.status = revision.status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            Revision revision = await GetById(id);

            _context.Revisions.Remove(revision);
            await _context.SaveChangesAsync();
        }
    }
}
