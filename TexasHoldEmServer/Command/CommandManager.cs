using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.Command
{
    public interface ICommandManager
    {
        void ProcessCommand(CommandTypeEnum commandType);
    }
    
    public class CommandManager : ICommandManager
    {
        
        public void ProcessCommand(CommandTypeEnum commandType)
        {
            
        }
    }
}