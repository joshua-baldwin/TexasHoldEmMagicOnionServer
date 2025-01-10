using THE.Shared.Enums;
using THE.Entities;

namespace THE.GameLogic
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
        int GetCurrentExtraBettingRound();
        int GetExtraBettingRoundsCount();
        List<PlayerEntity> GetAllPlayers();
        void SetupGame(List<PlayerEntity> players, bool isFirstRound);
        void DoAction(Enums.CommandTypeEnum commandType, int chipsBet, out bool isGameOver, out bool isError, out string actionMessage);
        void DiscardAndFinishUsingJoker(PlayerEntity target, JokerEntity joker, List<CardEntity> cardsToDiscard);
        void DiscardToCardPool(PlayerEntity target, List<CardEntity> cardsToDiscard);
        void DrawFromCardPool(PlayerEntity target, int numberOfCardsToDraw, bool addToTempCards); //draw random card
        void DrawFromCardPool(PlayerEntity target, List<CardEntity> cardsToDraw, bool duplicate); //draw specific card
        void CreateQueue(List<PlayerEntity> players);
        List<WinningHandEntity> DoShowdown();
        void AddJokerCostToPot(int cost);
        void UpdateQueue(PlayerEntity playerToChange);
        void IncreaseNumberOfBettingRounds();
        void UpdateCardWeight(List<CardEntity> card, int multiplier, bool increaseWeight);
        void LockCommunityCards(List<CardEntity> cardsToLock);
    }

    public interface IGameLogicManagerForTesting
    {
        void SetCommunityCardsForUnitTests(List<CardEntity> cards);
    }
}