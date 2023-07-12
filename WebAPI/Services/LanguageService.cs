using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly WebAPIContext _context;

        public LanguageService(WebAPIContext webAPIContext)
        {
            _context = webAPIContext;
        }

        public Task<List<Model.Language>> GetLanguage()
        {
            try
            {
              List<Model.Language> allLanguages = new List<Model.Language>();

                 allLanguages = _context.Languages.Select(c => new WebAPI.Model.Language()
                {
                    LangaugeId= c.LangaugeId,
                    Name = c.LanguageName
                 
                }).ToList();

                return Task.Run(() => allLanguages);


               
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

        public async Task<Model.Language> GetLanguageById(int languageId)
        {
            try
            {
                Model.Language modelLanguageData = new Model.Language();

                var languageFromDB=  await _context.Languages
                 .AsNoTracking().FirstOrDefaultAsync(e => e.LangaugeId == languageId);

                if (languageFromDB != null)
                {
                    modelLanguageData = new Model.Language
                    {
                        LangaugeId = languageFromDB.LangaugeId,
                        Name = languageFromDB.LanguageName
                    };

                }
                return modelLanguageData;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

        public Task<int> AddLanguage(Model.Language language)
        {
            try
            {
                Data.Language languageDataModel = new Data.Language
                {
                    LanguageName = language.Name
                };
           
                _context.Languages.Add(languageDataModel);

                return (Task.Run(() => _context.SaveChanges()));

            }
            catch (Exception ex)
            {
               return Task.FromResult(0);
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Delete an existing language from the database.
        /// </summary>
        /// <param name="language">The language to delete.</param>
        /// <returns>Task representing the asynchronous operation with the result of the deletion.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        public  Task<int> DeleteLanguage(Model.Language language)
        {
            try
            {
                Data.Language languageDataModel = new Data.Language
                {
                    LangaugeId = language.LangaugeId,
                    LanguageName = language.Name
                };


              
                List<Data.Command> commandFromDB =  _context.Commands
                    .Where(c => c.LanguageId == language.LangaugeId)
                    .Select(c => new Data.Command
                    {
                        CommandId = c.CommandId,
                        CommandDescription = c.CommandDescription,
                        CommandText = c.CommandText,
                        LanguageId = c.LanguageId
                    })
                    .ToList();

                _context.ChangeTracker.Clear();
                _context.Commands.RemoveRange(commandFromDB);
                _context.Languages.Remove(languageDataModel);

                return (Task.Run(() => _context.SaveChanges()));

            }
            catch (Exception ex)
            {
                return Task.FromResult(0);
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

        public async Task<Model.Language> UpdateLanguage(Model.Language language)
        {
            var result = await _context.Languages
                .FirstOrDefaultAsync(e => e.LangaugeId == language.LangaugeId);

            if (result != null)
            {
                result.LanguageName = language.Name;
               
                await _context.SaveChangesAsync();

                Model.Language languageDataModel = new Model.Language
                {
                    LangaugeId = result.LangaugeId,
                    Name = result.LanguageName
                };

                return languageDataModel;
            }

            return null;
        }

    }
}
