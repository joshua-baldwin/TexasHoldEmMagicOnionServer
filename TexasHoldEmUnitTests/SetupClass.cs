using THE.GameLogic;
using THE.Shared.Enums;
using THE.Entities;

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
            public List<PlayerEntity> Players { get; set; } 

            public List<CardEntity> GetHand(PlayerEntity player)
            {
                var newList = new List<CardEntity>();
                foreach (var card in GameLogicManager.GetCommunityCards())
                    newList.Add(new CardEntity(card.Suit, card.Rank));
                return player.HoleCards.Concat(new List<CardEntity>(newList)).ToList();
            }
        }

        public static PlayerEntity SetupTestHand(string name, Enums.PlayerRoleEnum role)
        {
            return new PlayerEntity(name, Guid.NewGuid(), role);
        }
        
        public static void SetNewHoleCards(PlayerEntity sb, List<CardEntity> cards)
        {
            sb.HoleCards.Clear();
            sb.MaxHoleCards = 0;
            foreach (var card in cards)
            {
                sb.HoleCards.Add(card);
                sb.MaxHoleCards++;
            }
        }

        public static TestSystem SetupAndDoBlindBet(List<(string Name, Enums.PlayerRoleEnum Role, Enums.CardSuitEnum Suit1, Enums.CardRankEnum Rank1, Enums.CardSuitEnum Suit2, Enums.CardRankEnum Rank2, int Chips)> playersToCreate, bool isFirstRound, out int totalChips, TestSystem sut = null)
        {
            var players = new List<PlayerEntity>();
            if (isFirstRound)
            {
                sut = new TestSystem();
                foreach (var player in playersToCreate)
                {
                    var p = SetupTestHand(player.Name, player.Role);
                    players.Add(p);
                }
            }
            else
            {
                players = sut.Players;
            }

            sut.GameLogicManager.SetupGame(players, isFirstRound);
            sut.Players = sut.GameLogicManager.GetAllPlayers();

            for (var i = 0; i < sut.Players.Count; i++)
            {
                var player = sut.Players[i];
                player.IsDealer = i == sut.Players.Count - 1;
                player.PlayerRole = playersToCreate[i].Role;
                player.Chips = playersToCreate[i].Chips;
            }

            totalChips = playersToCreate.Select(x => x.Chips).Sum();
            sut.GameLogicManager.CreateQueue(sut.Players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);
            
            Assert.That(sut.Players.All(player => player.CardsAreValid()));

            for (var i = 0; i < sut.Players.Count; i++)
                SetNewHoleCards(sut.Players[i], [new CardEntity(playersToCreate[i].Suit1, playersToCreate[i].Rank1), new CardEntity(playersToCreate[i].Suit2, playersToCreate[i].Rank2)]);

            return sut;
        }

        public static void AssertBeforeAction(TestSystem sut)
        {
            Assert.That(sut.Players.All(player => player.CardsAreValid()));
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

        public static void PurchaseAndUseJoker(TestSystem sut, int jokerToUse, PlayerEntity player, List<PlayerEntity> targets, List<CardEntity> cardsToDiscard, List<CardEntity> cardsToUpdateWeight, ref int totalChips)
        {
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == jokerToUse);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, player, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, player, targets, player.JokerCards.First(), cardsToDiscard, cardsToUpdateWeight, out var isJokerError, out _, out _);
            AssertAfterJokerAction(sut, totalChips, false, isJokerError);
        }
    }
}