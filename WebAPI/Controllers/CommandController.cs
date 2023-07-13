using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Windows.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : Controller
    {

        ICommandService CommandService = null;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Injecting a new instance of the ICommandService class.
        /// </summary>
        /// <param name="commandService">The command service dependency.</param>
        //-----------------------------------------------------------------------------------------

        public CommandController(ICommandService  commandService)
        {
            CommandService = commandService;
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
            try
            {
                List<CommandDTO> commands = new List<CommandDTO>();
                commands = await CommandService.GetCommands();
                if (commands.Count == 0)
                {
                    return NotFound($"Commands not found");
                }

                return Ok(commands);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
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
            try
            {
                CommandDTO command = new CommandDTO();
                command = await CommandService.GetCommandById(id);
             
                if (command == null || command.CommandId == 0)
                {
                    return NotFound($"Command with Id = {id} not found");
                }

                return Ok(command);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
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
            try
            {
                List<CommandDTO> commands = await CommandService.GetCommandsByLanguageId(languageId);

                if (commands == null || commands.Count == 0)
                {
                    return NotFound($"Commands with language id = {languageId} not found");
                }


                return Ok(commands);
            }
            catch (Exception)
            {
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
            try
            {
                List< CommandDTO> commands = await CommandService.GetCommandsByLanguageIdAndCommandId(languageId, commandId);

                if (commands == null || commands.Count == 0)
                {
                    return NotFound($"Commands with language id = {languageId} and command id = {commandId} not found");
                }

                return Ok(commands);
            }
            catch (Exception)
            {
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
            try
            {
                if (command == null)
                    return BadRequest();

                var createdCommand = await CommandService.AddCommand(command);

                return CreatedAtAction(nameof(GetCommandById),
                    new { id = command.CommandId }, createdCommand);
            }
            catch (Exception)
            {
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
        public async Task<ActionResult< CommandDTO>> UpdateCommand([FromBody]  CommandDTO command, int id)
        {
            try
            {
                if (id != command.CommandId)
                    return BadRequest("Command ID mismatch");

                var commandToUpdate = await CommandService.GetCommandById(id);

                if (commandToUpdate == null)
                    return NotFound($"Command with Id = {id} not found");

                return await CommandService.UpdateCommand(command);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


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
            try
            {
                 CommandDTO command = await CommandService.GetCommandById(id);
                if (command == null || command.CommandId == 0)
                {
                    return NotFound($"Command with Id = {id} not found");
                }

                await CommandService.DeleteCommand(command);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }



    }
}
