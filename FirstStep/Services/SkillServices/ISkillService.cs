using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISkillService
    {
        public Task<IEnumerable<Skill>> GetAll();

        public Task<Skill> GetById(int id);

        public Task<Skill?> GetByName(string skillName);

        public Task<IEnumerable<Skill>> SearchByName(string skillNamePattern);

        public Task Create(string newSkillName);

        public Task Update(int skillId, Skill skill);

        public Task Delete(int id);
    }
}
