using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class CommandService : ICommandService
    {
        private readonly WebAPIContext _context;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the web api context class.
        /// </summary>
        /// <param name="webAPIContext">The WebAPIContext used for data access.</param>
        //-----------------------------------------------------------------------------------------

        public CommandService(WebAPIContext webAPIContext)
        {
            _context = webAPIContext;
        } 


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Add a new command to the database.
        /// </summary>
        /// <param name="command">The command to add.</param>
        /// <returns>Task representing the asynchronous operation with the ID of the added command.</returns>
        //-----------------------------------------------------------------------------------------

        public  Task<int> AddCommand(Model.Command command)
        {
            try
            {
                var languageExists =  _context.Languages.Any(l => l.LangaugeId == command.LangaugeId);
                if (!languageExists)
                    throw new InvalidOperationException("The specified language ID does not exist.");

                Data.Command commandDataModel = new Data.Command
                {
                    CommandText = command.CommandText,
                    CommandDescription = command.CommandDescription,
                    LanguageId = command.LangaugeId
                };

                _context.Commands.Add(commandDataModel);

                return (Task.Run(() => _context.SaveChanges()));
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }


        /// <summary>
        /// Get all the commands.
        /// </summary>
        /// <returns>List of commands.</returns>
        /// <exception cref="Exception"></exception>
        
        public Task<List<Model.Command>> GetCommands()
        {
            try
            {
                List<Model.Command> allCommands = new List<Model.Command>();

                allCommands = _context.Commands.Select(c => new WebAPI.Model.Command()
                {
                    LangaugeId = c.LanguageId,
                    CommandText = c.CommandText,
                    CommandDescription = c.CommandDescription,
                    CommandId   =   c.CommandId

                }).ToList();

                return Task.Run(() => allCommands);



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

        public async Task<Model.Command> GetCommandById(int commandId)
        {
            try
            {
                Model.Command modelCommandData = new Model.Command();

                var commandFromDB = await _context.Commands
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.CommandId == commandId);

                if (commandFromDB != null)
                {
                    modelCommandData = new Model.Command
                    {
                        CommandId = commandFromDB.CommandId,
                        CommandText = commandFromDB.CommandText,
                        CommandDescription = commandFromDB.CommandDescription,
                        LangaugeId = commandFromDB.LanguageId
                    };
                }

                return modelCommandData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Update an existing command in the database.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <returns>Task representing the asynchronous operation with the updated command.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<Model.Command> UpdateCommand(Model.Command command)
        {
            var result = await _context.Commands
                .FirstOrDefaultAsync(e => e.CommandId == command.CommandId);

            var languageExists = _context.Languages.Any(l => l.LangaugeId == command.LangaugeId);
            if (!languageExists)
                throw new InvalidOperationException("The specified language ID does not exist.");

            if (result != null)
            {
                result.CommandText= command.CommandText;
                result.CommandDescription = command.CommandDescription;
                result.CommandId = command.CommandId;
                result.LanguageId = command.LangaugeId;

                await _context.SaveChangesAsync();

                Model.Command commandDataModel = new Model.Command
                {
                    CommandId = result.CommandId,
                    CommandText = result.CommandText,
                    CommandDescription = result.CommandDescription,
                    LangaugeId = result.LanguageId
                };

                return commandDataModel;
            }

            return null;
        }


        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Delete an existing command from the database.
        /// </summary>
        /// <param name="command">The command to delete.</param>
        /// <returns>Task representing the asynchronous operation with the result of the deletion.</returns>
        //-----------------------------------------------------------------------------------------

        public Task<int> DeleteCommand(Model.Command command)
        {
            try
            {
                Data.Command commandDataModel = new Data.Command
                {
                    CommandId = command.CommandId,
                    CommandDescription = command.CommandDescription,
                    CommandText = command.CommandText,
                    LanguageId = command.LangaugeId
                };

                _context.ChangeTracker.Clear();
                _context.Commands.Remove(commandDataModel);

                return (Task.Run(() => _context.SaveChanges()));
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Retrieve all commands by language ID from the database.
        /// </summary>
        /// <param name="languageId">The ID of the language to filter commands by.</param>
        /// <returns>List of commands filtered by the language ID.</returns>
        //-----------------------------------------------------------------------------------------

        public async Task<List<Model.Command>> GetCommandsByLanguageId(int languageId)
        {
            try
            {
                List<Model.Command> commands = await _context.Commands
                    .Where(c => c.LanguageId == languageId)
                    .Select(c => new Model.Command
                    {
                        CommandId = c.CommandId,
                        CommandText = c.CommandText,
                        CommandDescription = c.CommandDescription,
                        LangaugeId = c.LanguageId
                    })
                    .ToListAsync();

                return commands;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving commands by language ID", ex);
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

        public async Task<List<Model.Command>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId)
        {
            try
            {
                List<Model.Command> commands = await _context.Commands
                    .Where(c => c.LanguageId == languageId && c.CommandId == commandId)
                    .Select(c => new Model.Command
                    {
                        CommandId = c.CommandId,
                        CommandDescription = c.CommandDescription,
                        CommandText = c.CommandText,
                        LangaugeId = c.LanguageId
                    })
                    .ToListAsync();

                return commands;
            }
            catch (Exception ex)
            {
                // Handle any specific exception or log the error
                throw new Exception("Error retrieving commands by language ID and command ID", ex);
            }
        }

    }
}
