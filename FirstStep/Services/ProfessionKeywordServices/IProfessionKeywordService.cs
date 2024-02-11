using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IProfessionKeywordService
    {
        public Task<IEnumerable<string>> GetAll(int fieldID);

        public Task<ProfessionKeyword> GetById(int id);

        public Task<ProfessionKeyword?> GetByName(string name, int fieldID);

        public Task Create(ProfessionKeywordDto professionKeyword);
        
        public Task Update(int keywordID, ProfessionKeyword professionKeyword);
        
        public Task Delete(int id);
    }
}
