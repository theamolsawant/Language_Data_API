using DataAccessLayer;
using Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICommandService
    {
        /// <summary>
        /// Adds a new command.
        /// </summary>
        /// <param name="command">The command model to add.</param>
        /// <returns>The number of affected rows in the database after adding the command.
        /// </returns>
        Task<int> AddCommand(CommandDTO command);


        /// <summary>
        /// Updates an existing command.
        /// </summary>
        /// <param name="command">The updated command information.</param>
        /// <returns>The updated command details.</returns>
        Task<CommandDTO> UpdateCommand(CommandDTO command);
       
        
        /// <summary>
        /// Retrieves a command by its ID.
        /// </summary>
        /// <param name="commandId">The ID of the command to retrieve.</param>
        /// <returns>The command with the specified ID.</returns>
        Task<CommandDTO> GetCommandById(int commandId);


        /// <summary>
        /// Retrieves a list of commands.
        /// </summary>
        /// <returns>The list of commands.</returns>
        Task<List<CommandDTO>> GetCommands();

        /// <summary>
        /// Deletes a command.
        /// </summary>
        /// <param name="command">The command to delete.</param>
        /// <returns>The number of affected rows in the
        /// database after deleting the command.
        /// </returns>
        Task<int> DeleteCommand(CommandDTO command);

        /// <summary>
        /// Retrieves a list of commands associated
        /// with a specific language.
        /// </summary>
        /// <param name="languageId">The ID of
        /// the language.</param>
        /// <returns>The list of commands associated
        /// with the specified language ID.
        /// </returns>
        Task<List<CommandDTO>> GetCommandsByLanguageId(int languageId);
       
        
        /// <summary>
        /// Retrieves a list of commands associated with a specific language and command ID.
        /// </summary>
        /// <param name="languageId">The ID of the language.</param>
        /// <param name="commandId">The ID of the command.</param>
        /// <returns>The list of commands associated with the specified language ID and command ID.</returns>
        Task<List<CommandDTO>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId);

    }
}
