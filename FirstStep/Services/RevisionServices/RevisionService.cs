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
