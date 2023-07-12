using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Services
{
    public interface ILanguageService
    {
         Task<int> AddLanguage(Language language);

        Task<Language> UpdateLanguage(Language language);

        Task<Language> GetLanguageById (int languageId);

         Task<List<Language>> GetLanguage();

         Task<int> DeleteLanguage(Language language);

    }
}
