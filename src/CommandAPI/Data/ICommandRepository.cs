using CommandAPI.Models;

namespace CommandAPI.Data
{
    public interface ICommandRepository
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
        void CreateCommand(Command cmd);
        void EditCommand(Command cmd);
        void DeleteCommand(Command cmd);

    }
}
