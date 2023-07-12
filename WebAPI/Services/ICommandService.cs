using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Services
{
    public interface ICommandService
    {

        Task<int> AddCommand(Command command);

        Task<Command> UpdateCommand(Command command);

        Task<Command> GetCommandById(int commandId);

        Task<List<Command>> GetCommands();

        Task<int> DeleteCommand(Command command);

        Task<List<Model.Command>> GetCommandsByLanguageId(int languageId);

        Task<List<Model.Command>> GetCommandsByLanguageIdAndCommandId(int languageId, int commandId);

    }
}
