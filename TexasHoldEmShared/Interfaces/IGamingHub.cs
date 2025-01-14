using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;
using THE.Entities;
using THE.Shared.Enums;

namespace THE.Interfaces
{
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<Enums.JoinRoomResponseTypeEnum> JoinRoomAsync(string name);
        Task<PlayerEntity> LeaveRoomAsync();
        Task<PlayerEntity[]> GetAllPlayers();
        Task<Enums.StartResponseTypeEnum> StartGame(Guid playerId, bool isFirstRound);
        Task CancelStart(Guid playerId);
        Task<Enums.DoActionResponseTypeEnum> DoAction(Guid playerId, Enums.CommandTypeEnum commandType, int betAmount);
        Task<Enums.BuyJokerResponseTypeEnum> BuyJoker(Guid playerId, int jokerId);
        Task<Enums.UseJokerResponseTypeEnum> UseJoker(Guid jokerUserId, Guid selectedJokerUniqueId, List<Guid> targetPlayerIds, List<CardEntity> cardEntities);
        Task DiscardHoleCard(Guid jokerUserId, Guid selectedJokerUniqueId, List<CardEntity> card);
    }
}