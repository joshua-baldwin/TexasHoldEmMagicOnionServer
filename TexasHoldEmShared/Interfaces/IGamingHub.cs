using System;
using System.Threading.Tasks;
using MagicOnion;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<PlayerEntity> JoinRoomAsync(string name);
        Task<PlayerEntity> LeaveRoomAsync();
        Task<PlayerEntity[]> GetAllPlayers();
        Task StartGame(Guid playerId);
        Task CancelStart(Guid playerId);
        Task QuitGame(Guid playerId);
    }
}