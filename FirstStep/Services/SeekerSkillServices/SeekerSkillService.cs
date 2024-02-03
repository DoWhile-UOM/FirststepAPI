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
            return await _context.SeekerSkills.ToListAsync();
        }

        public async Task<SeekerSkill> GetById(int id)
        {
            var seekerSkill = await _context.SeekerSkills
                .Where(e => e.skill_id == id)
                .FirstOrDefaultAsync();

            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task<SeekerSkill> GetByName(string skillName)
        {
            var seekerSkill = await _context.SeekerSkills
                .Where(e => e.skill_name == skillName)
                .FirstOrDefaultAsync();

            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task Create(string newSeekerSkillName)
        {
            var seekerSkill = new SeekerSkill
            {
                skill_id = 0,
                skill_name = newSeekerSkillName
            };

            _context.SeekerSkills.Add(seekerSkill);
            await _context.SaveChangesAsync();
        }

        // not relavent for this controller

        public async Task Update(int id, SeekerSkill reqSeekerSkill)
        {
            var dbSeekerSkill = await GetById(id);

            dbSeekerSkill.skill_name = reqSeekerSkill.skill_name;

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
