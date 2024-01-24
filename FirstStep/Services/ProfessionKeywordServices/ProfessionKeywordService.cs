using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ProfessionKeywordService : IProfessionKeywordService
    {
        private readonly DataContext _context;

        public ProfessionKeywordService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfessionKeyword>> GetAll()
        {
            return await _context.ProfessionKeywords.Include(e => e.job_Field).ToListAsync();
        }

        public async Task<ProfessionKeyword> GetById(int id)
        {
            ProfessionKeyword? professionKeyword = await _context.ProfessionKeywords.FindAsync(id);
            if (professionKeyword is null)
            {
                // ask from abijan how to handle this exceptions
                throw new Exception("ProfessionKeyword not found.");
            }

            return professionKeyword;
        }

        public async Task<ProfessionKeyword> Create(ProfessionKeyword professionKeyword)
        {
            professionKeyword.profession_id = 0;

            _context.ProfessionKeywords.Add(professionKeyword);
            await _context.SaveChangesAsync();

            return professionKeyword;
        }

        public async void Update(ProfessionKeyword professionKeyword)
        {
            ProfessionKeyword dbProfessionKeyword = await GetById(professionKeyword.profession_id);

            dbProfessionKeyword.profession_name = professionKeyword.profession_name;

            await _context.SaveChangesAsync();
        }

        public async void Delete(int id)
        {
            ProfessionKeyword professionKeyword = await GetById(id);

            _context.ProfessionKeywords.Remove(professionKeyword);
            await _context.SaveChangesAsync();
        }
    }
}
