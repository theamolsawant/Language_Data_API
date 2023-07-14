using DataAccessLayer;
using Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ILanguageService
    {
        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="language">The language to add.</param>
        /// <returns>The number of affected rows in the 
        /// database after adding the language.
        /// </returns>
        Task<int> AddLanguage(LanguageDTO language);
        
        /// <summary>
        /// Updates an existing language.
        /// </summary>
        /// <param name="language">The updated language information.</param>
        /// <returns>The updated language details.</returns>
        Task<LanguageDTO> UpdateLanguage(LanguageDTO language);

        /// <summary>
        /// Retrieves a language by its ID.
        /// </summary>
        /// <param name="languageId">The ID of the language to retrieve.</param>
        /// <returns>The language with the specified ID.</returns>
        Task<LanguageDTO> GetLanguageById(int languageId);

        /// <summary>
        /// Retrieves a list of languages.
        /// </summary>
        /// <returns>A list of language DTOs representing the languages.</returns>
        Task<List<LanguageDTO>> GetLanguage();
        
        
        /// <summary>
        /// Deletes a language by its ID.
        /// </summary>
        /// <param name="languageID">The ID of the language to delete.</param>
        /// <returns>The number of affected rows in the database after deleting the language.</returns>
        Task<int> DeleteLanguage(int languageID);

    }
}
