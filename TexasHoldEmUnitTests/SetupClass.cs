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
            public List<PlayerEntity> Players { get; } = new List<PlayerEntity>(); 

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
                ],
                MaxHoleCards = 2,
            };
        }
        
        public static void SetNewHoleCards(PlayerEntity sb, CardEntity card1, CardEntity card2)
        {
            sb.HoleCards = [ card1, card2 ];
            sb.MaxHoleCards = 2;
        }

        public static TestSystem SetupAndDoBlindBet(List<(string Name, Enums.PlayerRoleEnum Role, Enums.CardSuitEnum Suit1, Enums.CardRankEnum Rank1, Enums.CardSuitEnum Suit2, Enums.CardRankEnum Rank2, int Chips)> playersToCreate, out int totalChips)
        {
            var sut = new TestSystem();
            foreach (var player in playersToCreate)
            {
                var p = SetupTestHand(player.Name, player.Role, player.Suit1, player.Rank1, player.Suit2, player.Rank2);
                sut.Players.Add(p);
            }
            
            sut.GameLogicManager.SetupGame(sut.Players, true);

            for (var i = 0; i < sut.Players.Count; i++)
            {
                var player = sut.Players[i];
                player.IsDealer = i == sut.Players.Count - 1;
                player.PlayerRole = playersToCreate[i].Role;
                player.Chips = playersToCreate[i].Chips;
                SetNewHoleCards(player, new CardEntity(playersToCreate[i].Suit1, playersToCreate[i].Rank1), new CardEntity(playersToCreate[i].Suit2, playersToCreate[i].Rank2));
            }

            totalChips = playersToCreate.Select(x => x.Chips).Sum();
            sut.GameLogicManager.CreateQueue(sut.Players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            foreach (var player in sut.Players)
                SetNewHoleCards(player, player.HoleCards[0], player.HoleCards[1]);

            return sut;
        }

        public static void AssertAfterAction(TestSystem sut, int totalChips, bool shouldBeError, bool isError, bool shouldBeGameOver, bool isGameOver)
        {
            Assert.That(shouldBeError, Is.EqualTo(isError));
            Assert.That(shouldBeGameOver, Is.EqualTo(isGameOver));
            Assert.That(sut.GameLogicManager.GetPots().Sum(x => x.PotAmount) + sut.GameLogicManager.GetAllPlayers().Sum(x => x.Chips), Is.EqualTo(totalChips));
            Assert.That(sut.GameLogicManager.GetAllPlayers().All(x => x.TempHoleCards.Count == 0 && x.MaxHoleCards == x.HoleCards.Count));
        }

        public static void AssertAfterJokerAction(TestSystem sut, int totalChips, bool shouldBeError, bool isError)
        {
            Assert.That(shouldBeError, Is.EqualTo(isError));
            Assert.That(sut.GameLogicManager.GetAllPlayers().All(x => x.TempHoleCards.Count == 0 && x.MaxHoleCards == x.HoleCards.Count));
            Assert.That(sut.GameLogicManager.GetPots().Sum(x => x.PotAmount) + sut.GameLogicManager.GetAllPlayers().Sum(x => x.Chips), Is.EqualTo(totalChips));
        }

        public static void PurchaseAndUseJoker(TestSystem sut, int jokerToUse, PlayerEntity player, List<PlayerEntity> targets, ref int totalChips)
        {
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == jokerToUse);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, player, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, player, targets, player.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _);
            AssertAfterJokerAction(sut, totalChips, false, isJokerError);
        }
    }
}