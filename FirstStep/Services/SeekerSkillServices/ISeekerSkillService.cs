using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISeekerSkillService
    {
        public Task<IEnumerable<SeekerSkill>> GetAll();

        public Task<SeekerSkill> GetById(int id);

        public Task<SeekerSkill> GetByName(string skillName);

        public Task Create(string newSeekerSkillName);

        public Task Update(int skillId, SeekerSkill seekerSkill);

        public Task Delete(int id);
    }
}
