using System;
using System.Collections.Generic;
using THE.Entities;
using THE.Shared.Enums;

namespace THE.Interfaces
{
    public interface IGamingHubReceiver
    {
        void OnJoinRoom(PlayerEntity player, int playerCount, List<JokerEntity> jokerEntities);
        void OnLeaveRoom(PlayerEntity player, int playerCount);
        void OnGetAllPlayers(List<PlayerEntity> playerEntities);
        void OnGameStart(List<PlayerEntity> playerEntities, PlayerEntity currentPlayer, Enums.GameStateEnum gameState, int roundNumber, bool isFirstRound);
        void OnCancelGameStart();
        void OnDoAction(Enums.CommandTypeEnum commandType, List<PlayerEntity> playerEntities, Guid previousPlayerId, Guid currentPlayerId, List<PotEntity> pots, List<CardEntity> communityCards, Enums.GameStateEnum gameState, int currentExtraBettingRound, bool isError, string actionMessage, List<WinningHandEntity> winnerList);
        void OnBuyJoker(PlayerEntity player, JokerEntity joker);
        void OnUseJoker(List<PlayerEntity> playerEntities, List<CardEntity> communityCards, PlayerEntity jokerUser, List<PlayerEntity> targets, JokerEntity joker, Guid currentPlayerId, List<PotEntity> pots, bool isError, bool showHand, string actionMessage);
        void OnDiscardHoleCard(PlayerEntity player, List<CardEntity> card, string actionMessage);
    }
}