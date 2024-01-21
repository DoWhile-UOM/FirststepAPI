using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IProfessionKeywordService
    {
        Task<IEnumerable<ProfessionKeyword>> GetAll();

        Task<ProfessionKeyword> GetById(int id);
        
        Task<ProfessionKeyword> Create(ProfessionKeyword professionKeyword);
        
        void Update(ProfessionKeyword professionKeyword);
        
        void Delete(int id);
    }
}
