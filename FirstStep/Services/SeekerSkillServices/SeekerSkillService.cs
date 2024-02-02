using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class SeekerSkillService : ISeekerSkillService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SeekerSkillService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeekerSkill>> GetAll()
        {
            return await _context.SeekerSkills.Include(e => e.job_Field).ToListAsync();
        }

        public async Task<SeekerSkill> GetById(int id)
        {
            var seekerSkill = await _context.SeekerSkills
                .Where(e => e.skill_id == id)
                .Include(e => e.job_Field)
                .FirstOrDefaultAsync();

            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task Create(SeekerSkillDto newSeekerSkill)
        {
            var seekerSkill = _mapper.Map<SeekerSkill>(newSeekerSkill);

            _context.SeekerSkills.Add(seekerSkill);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, SeekerSkill reqSeekerSkill)
        {
            var dbSeekerSkill = await GetById(id);

            dbSeekerSkill.skill_name = reqSeekerSkill.skill_name;
            dbSeekerSkill.field_id = reqSeekerSkill.field_id;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            SeekerSkill seekerSkill = await GetById(id);

            _context.SeekerSkills.Remove(seekerSkill);
            await _context.SaveChangesAsync();
        }
    }
}
