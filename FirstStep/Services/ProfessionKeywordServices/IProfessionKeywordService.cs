using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IProfessionKeywordService
    {
        Task<IEnumerable<ProfessionKeyword>> GetAll();

        Task<ProfessionKeyword> GetById(int id);
        
        Task Create(ProfessionKeywordDto professionKeyword);
        
        Task Update(int keywordID, ProfessionKeyword professionKeyword);
        
        Task Delete(int id);
    }
}
