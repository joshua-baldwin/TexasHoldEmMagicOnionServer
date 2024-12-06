using System;
using System.Threading.Tasks;
using MagicOnion;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<Enums.JoinRoomResponseTypeEnum> JoinRoomAsync(string name);
        Task<PlayerEntity> LeaveRoomAsync();
        Task<PlayerEntity[]> GetAllPlayers();
        Task<Enums.StartResponseTypeEnum> StartGame(Guid playerId, bool isFirstRound);
        Task CancelStart(Guid playerId);
        Task DoAction(Enums.CommandTypeEnum commandType, int betAmount, Guid targetPlayerId);
    }
}