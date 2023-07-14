using AutoMapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services
{
    public class LanguageService : ILanguageService
    {
        private readonly WebAPIContext _context;
        private readonly IMapper _mapper;
        public LanguageService(WebAPIContext webAPIContext,IMapper mapper)
        {
            _context = webAPIContext;
            _mapper = mapper;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieves the list of languages.
        /// </summary>
        /// <returns>The list of languages.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the list 
        /// of languages.
        /// </exception>
        //-----------------------------------------------------------------------------------------

        public async Task<List<LanguageDTO>> GetLanguage()
        {
            try
            {
                var languagesFromDB = await _context.Languages.ToListAsync();
                return _mapper.Map<List<LanguageDTO>>(languagesFromDB);

            }
            catch (Exception)
            {
                throw new Exception("Error retrieving languages");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve a language by its ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to retrieve.</param>
        /// <returns>Task representing the asynchronous operation with the language.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during retrieval.</exception>
        //-----------------------------------------------------------------------------------------

        public async Task<LanguageDTO> GetLanguageById(int languageId)
        {
            try
            {
                var languagesFromDB = await _context.Languages.FindAsync(languageId);

                return _mapper.Map<LanguageDTO>(languagesFromDB);

            }
            catch (Exception)
            {
                throw new Exception($"Error retrieving languages by languageID {languageId}");
            }
        }



        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Add a new language to the database.
        /// </summary>
        /// <param name="language">The language to add.</param>
        /// <returns>Task representing the asynchronous operation with the ID of the added language.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        public async Task<int> AddLanguage(LanguageDTO language)
        {
            try
            {
                Language languageDTO = _mapper.Map<Language>(language);
                _context.Languages.Add(languageDTO);

                return await Task.Run(() => _context.SaveChanges());

            }
            catch (Exception )
            {
                throw new Exception($"Error in adding language.");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Delete an existing language from the database.
        /// </summary>
        /// <param name="languageId">The language Id to delete the language.</param>
        /// <returns>Task representing the asynchronous operation with the result of the deletion.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        public async Task<int> DeleteLanguage(int langaugeId)
        {
            try
            {
                var languagesFromDB = await _context.Languages.FindAsync(langaugeId);

               var  commandFromDB =  _context.Commands
                    .Where(c => c.LanguageId == langaugeId)
                    .ToList();

                _context.ChangeTracker.Clear();
                _context.Commands.RemoveRange(commandFromDB);
                _context.Languages.Remove(languagesFromDB);

                return await Task.Run(() => _context.SaveChanges());
            }
            catch (Exception)
            {
                throw new Exception($"Error in deleting language.");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing language in the database.
        /// </summary>
        /// <param name="language">The updated language information.</param>
        /// <returns>Task representing the asynchronous operation with the updated language.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        public async Task<LanguageDTO> UpdateLanguage(LanguageDTO languageDto)
        {
            try
            {
                var result = await _context.Languages.FindAsync(languageDto.LangaugeId);

                result.LanguageName = languageDto.LanguageName;

                if (result != null)
                {
                    var res = await _context.SaveChangesAsync();
                    return _mapper.Map<LanguageDTO>(result); 
                }

                return null;
            }
            catch (Exception)
            {
                throw ;
            }
        }

    }
}
