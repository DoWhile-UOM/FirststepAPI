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
            // Get all revisions for the application
            var revisions = await _context.Revisions
                .Include(r => r.employee)
                .Where(r => r.application_id == applicationID)
                .OrderByDescending(r => r.date)
                .ToListAsync();

            if (!revisions.Any())
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            // Get the last revision
            var last_revision = revisions.FirstOrDefault();

            // Check if the latest revision was added by an HR Manager (HRM) or Company Admin (CA)
            if (last_revision != null && last_revision.employee != null)
            {
                var userType = last_revision.employee.user_type;
                if (userType == "HRM" || userType == "CA")
                {
                    return last_revision.status; // Valid status if added by HR Manager or Company Admin
                }
            }

            return last_revision?.status ?? ApplicationStatus.NotEvaluated.ToString();
        }



        //public async Task<string> GetCurrentStatus(int applicationID)
        //{
        //    Revision? last_revision = await _context.Revisions
        //        .Where(r => r.application_id == applicationID)
        //        .OrderByDescending(r => r.date)
        //        .FirstOrDefaultAsync();

        //    // need to check whether the latest revision is added by a HR manager or HR assistant
        //    // when it added by HR manager, the status is valid
        //    // when it added by HR assistant, need to check again any hr manager is added a revision before that


        //    if (last_revision is null)
        //    {
        //        return ApplicationStatus.NotEvaluated.ToString();
        //    }

        //    return last_revision.status;
        //}

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
            // Get the last revision for the application
            var revisions = await _context.Revisions
                .Where(r => r.application_id == revision.application_id)
                .OrderByDescending(r => r.date)
                .ToListAsync();

            // Check if the last revision was added by HRM or CA
            var last_revision = revisions.FirstOrDefault();
            if (last_revision != null)
            {
                if (last_revision.employee.user_type == "HRM" || last_revision.employee.user_type == "CA")
                {
                    if (revision.employee.user_type == "HRA")
                    {
                        throw new InvalidOperationException("HRA cannot evaluate this application as it was already evaluated by an HRM or CA.");
                    }
                }
            }

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
