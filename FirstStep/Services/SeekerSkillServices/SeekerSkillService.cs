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

        public SeekerSkillService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetAll()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<Skill> GetById(int id)
        {
            var seekerSkill = await _context.Skills
                .Where(e => e.skill_id == id)
                .FirstOrDefaultAsync();

            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task<Skill> GetByName(string skillName)
        {
            var seekerSkill = await _context.Skills
                .Where(e => e.skill_name == skillName)
                .FirstOrDefaultAsync();

            if (seekerSkill is null)
            {
                throw new Exception("SeekerSkill not found.");
            }

            return seekerSkill;
        }

        public async Task<IEnumerable<Skill>> SearchByName(string skillNamePattern)
        {
            return await _context.Skills
                .Where(e => e.skill_name.Contains(skillNamePattern))
                .ToListAsync();
        }

        public async Task Create(string newSeekerSkillName)
        {
            var seekerSkill = new Skill
            {
                skill_id = 0,
                skill_name = newSeekerSkillName
            };

            _context.Skills.Add(seekerSkill);
            await _context.SaveChangesAsync();
        }

        // not relavent for this controller

        public async Task Update(int id, Skill reqSeekerSkill)
        {
            var dbSeekerSkill = await GetById(id);

            dbSeekerSkill.skill_name = reqSeekerSkill.skill_name;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Skill seekerSkill = await GetById(id);

            _context.Skills.Remove(seekerSkill);
            await _context.SaveChangesAsync();
        }
    }
}
