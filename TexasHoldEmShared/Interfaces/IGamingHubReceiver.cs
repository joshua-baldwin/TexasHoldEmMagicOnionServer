using System;
using System.Collections.Generic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Interfaces
{
    public interface IGamingHubReceiver
    {
        void OnJoinRoom(PlayerEntity player, int playerCount, List<JokerEntity> jokerEntities);
        void OnLeaveRoom(PlayerEntity player, int playerCount);
        void OnGetAllPlayers(List<PlayerEntity> playerEntities);
        void OnGameStart(List<PlayerEntity> playerEntities, PlayerEntity currentPlayer, Enums.GameStateEnum gameState, int roundNumber, bool isFirstRound);
        void OnCancelGameStart();
        void OnDoAction(Enums.CommandTypeEnum commandType, List<PlayerEntity> playerEntities, Guid previousPlayerId, Guid currentPlayerId, List<PotEntity> pots, List<CardEntity> communityCards, Enums.GameStateEnum gameState, bool isError, string actionMessage, List<WinningHandEntity> winnerList);
        void OnBuyJoker(PlayerEntity player, JokerEntity joker);
        void OnUseJoker(PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity joker, List<PotEntity> pots, string actionMessage);
    }
}