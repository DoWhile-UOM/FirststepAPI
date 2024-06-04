using FirstStep.Data;
using FirstStep.Models;
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

        enum ApplicationStatus { Evaluated, NotEvaluated, Accepted, Rejected }

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

            // need to check whether the latest revision is added by a HR manager or HR assistant
            // when it added by HR manager, the status is valid
            // when it added by HR assistant, need to check again any hr manager is added a revision before that


            if (last_revision is null)
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            return last_revision.status;
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
        }

        public async Task Update(Revision revision)
        {
            Revision dbRevision = await GetById(revision.revision_id);

            dbRevision.comment = revision.comment;
            dbRevision.date = revision.date;
            dbRevision.status = revision.status;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Revision revision = await GetById(id);

            _context.Revisions.Remove(revision);
            await _context.SaveChangesAsync();
        }
    }
}
