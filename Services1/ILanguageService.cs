using DataAccessLayer;
using Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ILanguageService
    {
        Task<int> AddLanguage(LanguageDTO language);

        Task<LanguageDTO> UpdateLanguage(LanguageDTO language);

        Task<LanguageDTO> GetLanguageById(int languageId);

        Task<List<LanguageDTO>> GetLanguage();

        Task<int> DeleteLanguage(int languageID);

    }
}
