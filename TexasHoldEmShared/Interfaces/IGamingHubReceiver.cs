using System;
using System.Collections.Generic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHubReceiver
    {
        void OnJoinRoom(PlayerEntity player, int playerCount);
        void OnLeaveRoom(PlayerEntity player, int playerCount);
        void OnGetAllPlayers(PlayerEntity[] playerEntities);
        void OnGameStart(PlayerEntity[] playerEntities, PlayerEntity currentPlayer, Enums.GameStateEnum gameState, int roundNumber, bool isFirstRound);
        void OnCancelGameStart();
        void OnDoAction(Enums.CommandTypeEnum commandType, PlayerEntity[] playerEntities, Guid previousPlayerId, Guid currentPlayerId, Guid targetPlayerId, List<(Guid, int)> pots, List<CardEntity> communityCards, Enums.GameStateEnum gameState, bool isError, string actionMessage, List<WinningHandEntity> winnerList);
    }
}