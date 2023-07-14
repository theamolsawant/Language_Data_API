using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILogger<LanguageController> _logger;

        private ILanguageService _languageService { get; }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor injecting Service and logger.
        /// </summary>
        /// <param name="languageService">Language service</param>
        /// <param name="logger"></param>
        //-----------------------------------------------------------------------------------------

        public LanguageController(ILanguageService languageService, ILogger<LanguageController>  logger)
        {
            _languageService = languageService;
            _logger = logger;
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
            _logger.LogInformation("Processing request to get all languages.");
            try
            {
                List<LanguageDTO> languages = new List<LanguageDTO>();
                languages = await _languageService.GetLanguage();
                if (languages.Count == 0)
                {
                    return NoContent();
                }

                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving languages.{ex.Message}");

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
            _logger.LogInformation("Processing request to language  by id.");
            try
            {
                LanguageDTO language = new LanguageDTO();
                language = await _languageService.GetLanguageById(id);

                if (language == null || language.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }

                return Ok(language);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving languages by Id.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            };
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Create new langauge in the database.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        //-----------------------------------------------------------------------------------------

        // POST: api/Languages
        [HttpPost]
        public async Task<ActionResult> CreateLanguage([FromBody] LanguageDTO language)
        {
            _logger.LogInformation("Processing request to create new language.");
            try
            {
                if (language == null)
                    return BadRequest();

                var createdLanguage = await _languageService.AddLanguage(language);


                return CreatedAtAction(nameof(GetLanguage),
                    new { id = language.LangaugeId }, createdLanguage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating new  language.{ex.Message}");
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
            _logger.LogInformation("Processing request to update language by id.");
            try
            {
                if (id != language.LangaugeId)
                    return BadRequest("Language ID mismatch");

                var languageToUpdate = await _languageService.GetLanguageById(id);


                if (languageToUpdate == null || languageToUpdate.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }

                return await _languageService.UpdateLanguage(language);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating existing language.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error in updating language:{ex.Message}");
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
            _logger.LogInformation("Processing request to delete language by id.");
            try
            {
                LanguageDTO language = await _languageService.GetLanguageById(id);
                if (language == null || language.LangaugeId == 0)
                {
                    return NotFound($"Language with Id = {id} not found");
                }

                await _languageService.DeleteLanguage(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting language.{ex.Message}");
                return StatusCode(500, "An error occurred while updating the language record.");
            }
        }

    }
}
