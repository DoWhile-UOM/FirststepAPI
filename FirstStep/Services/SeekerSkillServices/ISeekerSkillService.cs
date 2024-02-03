using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISeekerSkillService
    {
        public Task<IEnumerable<Skill>> GetAll();

        public Task<Skill> GetById(int id);

        public Task<Skill> GetByName(string skillName);

        public Task<IEnumerable<Skill>> SearchByName(string skillNamePattern);

        public Task Create(string newSeekerSkillName);

        public Task Update(int skillId, Skill seekerSkill);

        public Task Delete(int id);
    }
}
