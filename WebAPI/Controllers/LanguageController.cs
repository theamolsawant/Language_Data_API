using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Model;



namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {

        ILanguageService LanguageService = null;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor injecting Service.
        /// </summary>
        /// <param name="languageService">Language service</param>
        //-----------------------------------------------------------------------------------------

        public LanguageController(ILanguageService languageService)
        {
            LanguageService = languageService;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Get all languages from the database.
        /// </summary>
        /// <returns>Action result representing the response containing the languages.</returns>
        //-----------------------------------------------------------------------------------------

        // GET: api/Languages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageDTO>>> GetLanguage()
        {
            try
            {
                List<LanguageDTO> languages = new List<LanguageDTO>();
                languages = await LanguageService.GetLanguage();
                if (languages.Count == 0)
                {
                    return NoContent();
                }

                return Ok(languages);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
     "Error retrieving data from the database");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Get a language by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the language to retrieve.</param>
        /// <returns>Action result representing the response containing the language.</returns>
        //-----------------------------------------------------------------------------------------

        // GET: api/Languages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageDTO>> GetLanguageById(int id)
        {
            try
            {
                LanguageDTO language = new LanguageDTO();
                language = await LanguageService.GetLanguageById(id);

                if (language == null || language.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }

                return Ok(language);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            };
        }



        /// <summary>
        /// Create new langauge in the database.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>

        // POST: api/Languages
        [HttpPost]
        public async Task<ActionResult> CreateLanguage([FromBody] LanguageDTO language)
        {
            try
            {
                if (language == null)
                    return BadRequest();

                var createdLanguage = await LanguageService.AddLanguage(language);


                return CreatedAtAction(nameof(GetLanguage),
                    new { id = language.LangaugeId }, createdLanguage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing language in the database.
        /// </summary>
        /// <param name="language">The updated language information.</param>
        /// <param name="id">The ID of the language to update.</param>
        /// <returns>Action result representing the response containing the updated language.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        [HttpPut("{id:int}")]
        public async Task<ActionResult<LanguageDTO>> UpdateLanguage([FromBody] LanguageDTO language, int id)
        {
            try
            {
                if (id != language.LangaugeId)
                    return BadRequest("Language ID mismatch");

                var languageToUpdate = await LanguageService.GetLanguageById(id);


                if (languageToUpdate == null || languageToUpdate.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }


                return await LanguageService.UpdateLanguage(language);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Deletes a language record by its ID.
        /// </summary>
        /// <param name="id">The ID of the language record to delete.</param>
        /// <returns>An action result indicating the success or failure of the deletion.
        /// </returns>
        //-----------------------------------------------------------------------------------------

        // DELETE: api/Languages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            try
            {
                LanguageDTO language = await LanguageService.GetLanguageById(id);
                if (language == null || language.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }

                await LanguageService.DeleteLanguage(id);


                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the language record.");
            }
        }



    }
}
