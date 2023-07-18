using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.AutoMapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : Controller
    {
        private ICommandService _commandService { get; }

        private readonly ILogger<CommandController> _logger;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Injecting a new instance of the ICommandService class and logger.
        /// </summary>
        /// <param name="commandService">The command service dependency.</param>
        /// <param name="logger"></param>
        //-----------------------------------------------------------------------------------------

        public CommandController(ICommandService  commandService , ILogger<CommandController> logger)
        {
            _commandService = commandService;
            _logger = logger;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands from the database.
        /// </summary>
        /// <returns>Action result representing the response containing all commands.</returns>
        // GET: api/Commands
        //-----------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommands()
        {
            _logger.LogInformation("Processing request to get commands.");
            try
            {
                List<CommandDTO> commands = new List<CommandDTO>();
                commands = await _commandService.GetCommands();
                if (commands.Count == 0)
                {
                    return NotFound($"Commands not found");
                }

                return Ok(commands);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving commands.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while retrieving commands.");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve a command by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the command to retrieve.</param>
        /// <returns>Action result representing the response containing the requested command.</returns>
        //-----------------------------------------------------------------------------------------

        // GET: api/Commands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandDTO>> GetCommandById(int id)
        {
            _logger.LogInformation("Processing request to get command by Id");
            try
            {
                CommandDTO command = new CommandDTO();
                command = await _commandService.GetCommandById(id);
             
                if (command == null || command.CommandId == 0)
                {
                    return NotFound($"Command with Id = {id} not found");
                }

                return Ok(command);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving command by id.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving commands from the database");
            }
        }



        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands by language ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to filter commands by.</param>
        /// <returns>Action result representing the response containing the commands.</returns>
        //-----------------------------------------------------------------------------------------

        // GET: api/Commands/ByLanguage/5
        [HttpGet("ByLanguage/{languageId}")]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommandsByLanguageId(int languageId)
        {
            _logger.LogInformation("Processing request to get commands by language Id.");
            try
            {
                List<CommandDTO> commands = await _commandService.GetCommandsByLanguageId(languageId);

                if (commands == null || commands.Count == 0)
                {
                    return NotFound($"Commands with language id = {languageId} not found");
                }


                return Ok(commands);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while  retrieving commands by language Id.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands by language ID and command ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to filter commands by.</param>
        /// <param name="commandId">The ID of the command to filter commands by.</param>
        /// <returns>Action result representing the response containing the commands.</returns>
        //-----------------------------------------------------------------------------------------

        // GET: api/Commands/ByLanguageAndCommand/{languageId}/{commandId}
        [HttpGet("ByLanguageAndCommand/{languageId}/{commandId}")]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId)
        {
            _logger.LogInformation("Processing request to get commands by language id and command id.");
            try
            {
                List< CommandDTO> commands = await _commandService.GetCommandsByLanguageIdAndCommandId(languageId, commandId);

                if (commands == null || commands.Count == 0)
                {
                    return NotFound($"Commands with language id = {languageId} and command id = {commandId} not found");
                }

                return Ok(commands);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving commands by command Id and language Id{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }



        //-----------------------------------------------------------------------------------------
        // POST: api/Commands
        /// <summary>
        /// Create a new command in the database.
        /// </summary>
        /// <param name="command">The command to create.</param>
        /// <returns>Action result representing the response of the creation operation.</returns>
        //-----------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> CreateCommand([FromBody]  CommandDTO command)
        {
            _logger.LogInformation("Processing request to create new command.");
            try
            {
                if (command == null)
                    return BadRequest();

                var createdCommand = await _commandService.AddCommand(command);

                return CreatedAtAction(nameof(GetCommandById),
                    new { id = command.CommandId }, createdCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating new command.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new command record");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing command in the database.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <param name="id">The ID of the command to update.</param>
        /// <returns>Action result representing the response containing the updated command.</returns>
        //-----------------------------------------------------------------------------------------

        [HttpPut("{id:int}")]
        public async Task<ActionResult< CommandDTO>> UpdateCommandPut([FromBody]  CommandDTO command, int id)
        {
            _logger.LogInformation("Processing request to update existing command by command id.");
            try
            {
                if (id != command.CommandId)
                    return BadRequest("Command ID mismatch");

                var commandToUpdate = await _commandService.GetCommandById(id);

                if (commandToUpdate == null)
                    return NotFound($"Command with Id = {id} not found");
                
                return await _commandService.UpdateCommandPut(command);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating existing command.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error in updating command: {ex.Message}" );
            }
        }
        
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing command in the database.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <param name="id">The ID of the command to update.</param>
        /// <returns>Action result representing the response containing the updated command.</returns>
        //-----------------------------------------------------------------------------------------

        [HttpPatch("{id:int}")]
        public async Task<ActionResult< CommandDTO>> UpdateCommandPatch([FromBody]  CommandDTO command, int id)
        {
            _logger.LogInformation("Processing request to update existing command by command id.");
            try
            {
                if (id != command.CommandId)
                    return BadRequest("Command ID mismatch");

                var commandToUpdate = await _commandService.GetCommandById(id);

                if (commandToUpdate == null)
                    return NotFound($"Command with Id = {id} not found");
                
                return await _commandService.UpdateCommandPatch(command);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating existing command.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error in updating command: {ex.Message}" );
            }
        }
        
       

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Partially update an existing command in the database.
        /// </summary>
        /// <param name="id">The ID of the command to update.</param>
        /// <param name="patchDocument">The JSON patch document containing the updates.</param>
        /// <returns>Action result representing the response containing the updated command.</returns>
        //-----------------------------------------------------------------------------------------

  
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Delete an existing command from the database.
        /// </summary>
        /// <param name="id">The ID of the command to delete.</param>
        /// <returns>Task representing the asynchronous operation with the result of the deletion.</returns>
        //-----------------------------------------------------------------------------------------

        // DELETE: api/Commands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            _logger.LogInformation("Processing request to delete command by Id.");
            try
            {
                 CommandDTO command = await _commandService.GetCommandById(id);
                if (command == null || command.CommandId == 0)
                {
                    return NotFound($"Command with Id = {id} not found");
                }

                await _commandService.DeleteCommand(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting existing command by it's Id.{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

    }
}
