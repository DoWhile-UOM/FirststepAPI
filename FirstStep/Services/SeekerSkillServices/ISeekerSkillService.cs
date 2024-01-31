using FirstStep.Models;

namespace FirstStep.Services
{
    public interface ISeekerSkillService
    {
        public Task<IEnumerable<SeekerSkill>> GetAll();

        public Task<SeekerSkill> GetById(int id);

        public Task Create(SeekerSkill seekerSkill);

        public Task Update(SeekerSkill seekerSkill);

        public Task Delete(int id);
    }
}
