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
                ],
                MaxHoleCards = 2,
            };
        }
        
        public static void SetNewHoleCards(PlayerEntity sb, CardEntity card1, CardEntity card2)
        {
            sb.HoleCards = [ card1, card2 ];
            sb.MaxHoleCards = 2;
        }

        public static TestSystem SetupAndDoBlindBet(List<Enums.PlayerRoleEnum> roles, List<int> chips, List<List<CardEntity>> cards)
        {
            var sut = new TestSystem();
            var sb = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
            var bb = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
            var none3 = SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var none4 = SetupTestHand("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var none5 = SetupTestHand("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var none6 = SetupTestHand("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            
            
            var players = new List<PlayerEntity> { sb, bb, none3, none4, none5, none6 };
            sut.GameLogicManager.SetupGame(players, true);

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.IsDealer = false;
                player.PlayerRole = roles[i];
                player.Chips = chips[i];
                SetNewHoleCards(player, cards[i][0], cards[i][1]);
            }

            var totalChips = chips.Sum();
            sut.GameLogicManager.CreateQueue(players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            SetNewHoleCards(sb, new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine));
            SetNewHoleCards(bb, new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack), new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine));
            SetNewHoleCards(none3, new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two), new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five));
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

        public static void PurchaseAndUseJoker(TestSystem sut, int jokerToUse, PlayerEntity player, ref int totalChips)
        {
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == jokerToUse);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, player, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, player, [player], player.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _);
            AssertAfterJokerAction(sut, totalChips, false, isJokerError);
        }
    }
}