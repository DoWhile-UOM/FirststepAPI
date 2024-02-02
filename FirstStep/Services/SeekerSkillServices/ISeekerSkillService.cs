using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISeekerSkillService
    {
        public Task<IEnumerable<SeekerSkill>> GetAll();

        public Task<SeekerSkill> GetById(int id);

        public Task Create(SeekerSkillDto seekerSkill);

        public Task Update(int skillId, SeekerSkill seekerSkill);

        public Task Delete(int id);
    }
}
