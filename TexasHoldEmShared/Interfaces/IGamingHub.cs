using System;
using System.Threading.Tasks;
using MagicOnion;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<bool> JoinRoomAsync(string name);
        Task<PlayerEntity> LeaveRoomAsync();
        Task<PlayerEntity[]> GetAllPlayers();
        Task<bool> StartGame(Guid playerId);
        Task CancelStart(Guid playerId);
        Task DoAction(Enums.CommandTypeEnum commandType, int betAmount, Guid targetPlayerId);
    }
}