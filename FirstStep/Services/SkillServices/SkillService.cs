using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class SkillService : ISkillService
    {
        private readonly DataContext _context;

        public SkillService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetAll()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<Skill> GetById(int id)
        {
            var skill = await _context.Skills
                .Where(e => e.skill_id == id)
                .FirstOrDefaultAsync();

            if (skill is null)
            {
                throw new Exception("Skill not found.");
            }

            return skill;
        }

        public async Task<Skill?> GetByName(string skillName)
        {
            var skill = await _context.Skills
                .Where(e => e.skill_name == skillName)
                .FirstOrDefaultAsync();

            return skill;
        }

        public async Task<IEnumerable<Skill>> SearchByName(string skillNamePattern)
        {
            return await _context.Skills
                .Where(e => e.skill_name.Contains(skillNamePattern))
                .ToListAsync();
        }

        public async Task Create(string newskillName)
        {
            if (await GetByName(newskillName) != null)
                throw new Exception("Skill already exists.");

            var skill = new Skill
            {
                skill_id = 0,
                skill_name = newskillName
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }

        // not relavent for this controller

        public async Task Update(int id, Skill reqskill)
        {
            var dbskill = await GetById(id);

            dbskill.skill_name = reqskill.skill_name;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Skill skill = await GetById(id);

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
        }
    }
}
