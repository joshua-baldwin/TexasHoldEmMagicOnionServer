using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.GameLogic
{
    public interface IGameLogicManager
    {
        Queue<PlayerEntity> GetPlayerQueue();
        PlayerEntity GetPreviousPlayer();
        PlayerEntity GetCurrentPlayer();
        List<PotEntity> GetPots();
        List<CardEntity> GetCommunityCards();
        Enums.GameStateEnum GetGameState();
        int GetCurrentRound();
        List<PlayerEntity> GetAllPlayers();
        void Reset();
        void SetupGame(List<PlayerEntity> players, bool isFirstRound);
        void DoAction(Enums.CommandTypeEnum commandType, int chipsBet, out bool isGameOver, out bool isError, out string actionMessage);
        void DiscardToCardPool(PlayerEntity target, List<CardEntity> cardsToDiscard);
        List<CardEntity> DrawFromCardPool(int numberOfCardsToDraw);
        void CreateQueue(List<PlayerEntity> players);
        List<WinningHandEntity> DoShowdown();
        void AddJokerCostToPot(int cost);
    }

    public interface IGameLogicManagerForTesting
    {
        void SetCommunityCardsForUnitTests(List<CardEntity> cards);
    }
}