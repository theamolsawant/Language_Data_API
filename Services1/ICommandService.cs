using DataAccessLayer;
using Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICommandService
    {

        Task<int> AddCommand(CommandDTO command);

        Task<CommandDTO> UpdateCommand(CommandDTO command);

        Task<CommandDTO> GetCommandById(int commandId);

        Task<List<CommandDTO>> GetCommands();

        Task<int> DeleteCommand(CommandDTO command);

        Task<List<CommandDTO>> GetCommandsByLanguageId(int languageId);

        Task<List<CommandDTO>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId);

    }
}
