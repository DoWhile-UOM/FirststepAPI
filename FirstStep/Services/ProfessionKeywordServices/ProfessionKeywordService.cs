using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ProfessionKeywordService : IProfessionKeywordService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProfessionKeywordService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAll(int fieldID)
        {
            IEnumerable<string> keywords = await _context.ProfessionKeywords.Where(e => e.field_id == fieldID).Select(e => e.profession_name).ToListAsync();
            return keywords;
        }

        public async Task<ProfessionKeyword> GetById(int id)
        {
            var professionKeyword = await _context.ProfessionKeywords
                .Where(e => e.profession_id == id)
                .Include(e => e.job_Field)
                .FirstOrDefaultAsync();

            if (professionKeyword is null)
            {
                throw new Exception("ProfessionKeyword not found.");
            }

            return professionKeyword;
        }

        public async Task<ProfessionKeyword?> GetByName(string name, int fieldID)
        {
            var professionKeyword = await _context.ProfessionKeywords
                .Where(e => e.profession_name == name && e.field_id == fieldID)
                .Include(e => e.job_Field)
                .FirstOrDefaultAsync();

            return professionKeyword;
        }

        public async Task Create(ProfessionKeywordDto newProfessionKeyword)
        {
            var professionKeyword = _mapper.Map<ProfessionKeyword>(newProfessionKeyword);

            _context.ProfessionKeywords.Add(professionKeyword);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int keywordID, ProfessionKeyword reqProfessionKeyword)
        {
            var dbProfessionKeyword = await GetById(keywordID);

            dbProfessionKeyword.profession_name = reqProfessionKeyword.profession_name;
            dbProfessionKeyword.field_id = reqProfessionKeyword.field_id;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var professionKeyword = await GetById(id);

            _context.ProfessionKeywords.Remove(professionKeyword);
            await _context.SaveChangesAsync();
        }
    }
}
