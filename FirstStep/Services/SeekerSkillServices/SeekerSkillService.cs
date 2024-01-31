using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class SeekerSkillService : ISeekerSkillService
    {
        private readonly DataContext _context;

        public SeekerSkillService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeekerSkill>> GetAll()
        {
            return await _context.SeekerSkills.ToListAsync();
        }

        public async Task<SeekerSkill> GetById(int id)
        {
            SeekerSkill? seekerSkill = await _context.SeekerSkills.FindAsync(id);
            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task Create(SeekerSkill seekerSkill)
        {
            seekerSkill.skill_id = 0;

            _context.SeekerSkills.Add(seekerSkill);
            await _context.SaveChangesAsync();
        }

        public async Task Update(SeekerSkill seekerSkill)
        {
            SeekerSkill dbSeekerSkill = await GetById(seekerSkill.skill_id);

            dbSeekerSkill.skill_id = seekerSkill.skill_id;
            dbSeekerSkill.skill_name = seekerSkill.skill_name;

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
