using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.Command
{
    public interface ICommandManager
    {
        void ProcessCommand(Enums.CommandTypeEnum commandType);
    }
    
    public class CommandManager : ICommandManager
    {
        
        public void ProcessCommand(Enums.CommandTypeEnum commandType)
        {
            
        }
    }
}