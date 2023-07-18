using AutoMapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CommandService : ICommandService
    {
        private readonly WebAPIContext _context;
        private readonly IMapper _mapper;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the web api context class.
        /// </summary>
        /// <param name="webAPIContext">The WebAPIContext used for data access.</param>
        //-----------------------------------------------------------------------------------------

        public CommandService(WebAPIContext webAPIContext,IMapper mapper)
        {
            _context = webAPIContext;
            _mapper = mapper;
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Add a new command to the database.
        /// </summary>
        /// <param name="command">The command to add.</param>
        /// <returns>Task representing the asynchronous operation with the ID of the added command.</returns>
        //-----------------------------------------------------------------------------------------

        public Task<int> AddCommand(CommandDTO command)
        {
            try
            {
                var languageExists = _context.Languages.Any(l => l.LangaugeId == command.LanguageId);
                if (!languageExists)
                    throw new InvalidOperationException("The specified language ID does not exist.");

                Command commandDataModel = new Command
                {
                    CommandText = command.CommandText,
                    CommandDescription = command.CommandDescription,
                    LanguageId = command.LanguageId
                };

                _context.Commands.Add(commandDataModel);

                return Task.Run(() => _context.SaveChanges());
            }
            catch (Exception)
            {
                throw new Exception("Error adding command.");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Get all the commands.
        /// </summary>
        /// <returns>List of commands.</returns>
        /// <exception cref="Exception"></exception>
        //-----------------------------------------------------------------------------------------

        public async Task<List<CommandDTO>> GetCommands()
        {
            try
            {
                var commandsFromDB = await _context.Commands.ToListAsync();
                return _mapper.Map<List<CommandDTO>>(commandsFromDB);
            }
            catch (Exception )
            {
                throw new Exception("Error retrieving commands");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Get a command by its ID from the database.
        /// </summary>
        /// <param name="commandId">The ID of the command to retrieve.</param>
        /// <returns>Task representing the asynchronous operation with the command.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during retrieval.</exception>
        //-----------------------------------------------------------------------------------------

        public async Task<CommandDTO> GetCommandById(int commandId)
        {
            try
            {
                var commandsFromDB = await _context.Commands.FindAsync(commandId);

                return _mapper.Map<CommandDTO>(commandsFromDB);

            }
            catch (Exception)
            {
                throw new Exception("Error retrieving command by ID ");
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing command in the database.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <returns>Task representing the asynchronous operation with the updated command.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<CommandDTO> UpdateCommandPut(CommandDTO command)
        {
            try
            {
                bool languageExists = _context.Languages.Any(l => l.LangaugeId == command.LanguageId);
                if (!languageExists)
                    throw new InvalidOperationException("The specified language ID does not exists.");

                var result = await _context.Commands.FindAsync(command.CommandId);


                if (result != null)
                {
                        result.CommandText = command.CommandText;
                        result.CommandDescription = command.CommandDescription;
                        result.CommandDescription = command.CommandDescription;
                        result.CommandId = command.CommandId;
                        result.LanguageId = command.LanguageId;

                    var res = await _context.SaveChangesAsync();
                    return _mapper.Map<CommandDTO>(result);
                }

                return null;
            }
            catch (Exception)
            {
                throw ;
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing command in the database with patch.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <returns>Task representing the asynchronous operation with the updated command.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<CommandDTO> UpdateCommandPatch(CommandDTO command)
        {
            try
            {
                bool languageExists = _context.Languages.Any(l => l.LangaugeId == command.LanguageId);
                if (!languageExists)
                    throw new InvalidOperationException("The specified language ID does not exists.");

                var result = await _context.Commands.FindAsync(command.CommandId);


                if (result != null)
                {
                     if (!string.IsNullOrEmpty(command.CommandText))
                    result.CommandText = command.CommandText;

                    if (!string.IsNullOrEmpty(command.CommandDescription))
                    result.CommandDescription = command.CommandDescription;

                    result.CommandId = command.CommandId;
                    result.LanguageId = command.LanguageId;

                    var res = await _context.SaveChangesAsync();
                    return _mapper.Map<CommandDTO>(result);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Delete an existing command from the database.
        /// </summary>
        /// <param name="command">The command to delete.</param>
        /// <returns>Task representing the asynchronous operation with the result of the deletion.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<int> DeleteCommand(CommandDTO command)
        {
            try
            {
                var t = _mapper.Map<Command>(command);
                _context.ChangeTracker.Clear();
                _context.Commands.Remove(t);

                return await Task.Run(() => _context.SaveChanges());
            }
            catch (Exception)
            {
                throw new Exception("Error deleting command command ID");

            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands by language ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to filter commands by.</param>
        /// <returns>List of commands filtered by the language ID.</returns>
        //-----------------------------------------------------------------------------------------

        public Task<List<CommandDTO>> GetCommandsByLanguageId(int languageId)
        {
            try
            {
                List<Command> commandFromDB = _context.Commands
                    .Where(c => c.LanguageId == languageId)
                    .ToList();

                return Task.FromResult(_mapper.Map<List<CommandDTO>>(commandFromDB));
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving commands by language ID");
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands by language ID and command ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to filter commands by.</param>
        /// <param name="commandId">The ID of the command to filter commands by.</param>
        /// <returns>List of commands filtered by the language ID and command ID.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<List<CommandDTO>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId)
        {
            try
            {
                List<Command> commands = await _context.Commands
                    .Where(c => c.LanguageId == languageId && c.CommandId == commandId)
                    .ToListAsync();

                return _mapper.Map<List<CommandDTO>>(commands); ;
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving commands by language ID and command ID");
            }
        }

    }
}
