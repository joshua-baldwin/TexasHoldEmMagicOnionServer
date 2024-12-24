using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests
{
    public static class SetupClass
    {
        public class TestGameLogicManager : GameLogicManager, IGameLogicManagerForTesting
        {
            public void SetCommunityCardsForUnitTests(List<CardEntity> cards)
            {
                communityCards = cards;
            }
        }

        public class TestSystem
        {
            public TestGameLogicManager GameLogicManager { get; } = new TestGameLogicManager();
            public JokerManager JokerManager { get; } = new JokerManager();

            public List<CardEntity> GetHand(PlayerEntity player)
            {
                var newList = new List<CardEntity>();
                foreach (var card in GameLogicManager.GetCommunityCards())
                    newList.Add(new CardEntity(card.Suit, card.Rank));
                return player.HoleCards.Concat(new List<CardEntity>(newList)).ToList();
            }
        }

        public static PlayerEntity SetupTestHand(string name, Enums.PlayerRoleEnum role, Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank, Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank)
        {
            return new PlayerEntity(name, Guid.NewGuid(), role)
            {
                HoleCards =
                [
                    new CardEntity(holeCard1Suit, holeCard1Rank),
                    new CardEntity(holeCard2Suit, holeCard2Rank)
                ]
            };
        }

        public static void AssertAfterAction(TestSystem sut, int totalChips, bool shouldBeError, bool isError, bool shouldBeGameOver, bool isGameOver)
        {
            Assert.That(shouldBeError, Is.EqualTo(isError));
            Assert.That(shouldBeGameOver, Is.EqualTo(isGameOver));
            Assert.That(sut.GameLogicManager.GetPots().Sum(x => x.PotAmount) + sut.GameLogicManager.GetAllPlayers().Sum(x => x.Chips), Is.EqualTo(totalChips));
        }

        public static void AssertAfterJokerAction(TestSystem sut, int totalChips, bool shouldBeError, bool isError)
        {
            Assert.That(shouldBeError, Is.EqualTo(isError));
            Assert.That(sut.GameLogicManager.GetPots().Sum(x => x.PotAmount) + sut.GameLogicManager.GetAllPlayers().Sum(x => x.Chips), Is.EqualTo(totalChips));
        }
    }
}