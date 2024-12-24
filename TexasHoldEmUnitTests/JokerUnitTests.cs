using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests
{
    public partial class Tests
    {
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void PlayerUsesHandInfluenceJokerTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6)
        {
            var sut = new SetupClass.TestSystem();
            var p1 = SetupClass.SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
            var p2 = SetupClass.SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
            var p3 = SetupClass.SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p4 = SetupClass.SetupTestHand("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p5 = SetupClass.SetupTestHand("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p6 = SetupClass.SetupTestHand("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);

            var players = new List<PlayerEntity> { p1, p2, p3, p4, p5, p6 };
            sut.GameLogicManager.SetupGame(players, true);
            var sb = players.First(x => x.Name == "small");
            sb.IsDealer = false;
            sb.PlayerRole = role1;
            sb.Chips = chip1;

            var bb = players.First(x => x.Name == "big");
            bb.IsDealer = false;
            bb.PlayerRole = role2;
            bb.Chips = chip2;

            var none3 = players.First(x => x.Name == "none3");
            none3.IsDealer = false;
            none3.PlayerRole = role3;
            none3.Chips = chip3;

            var none4 = players.First(x => x.Name == "none4");
            none4.IsDealer = false;
            none4.PlayerRole = role4;
            none4.Chips = chip4;

            var none5 = players.First(x => x.Name == "none5");
            none5.IsDealer = false;
            none5.PlayerRole = role5;
            none5.Chips = chip5;

            var none6 = players.First(x => x.Name == "none6");
            none6.IsDealer = true;
            none6.PlayerRole = role6;
            none6.Chips = chip6;

            var totalChips = chip1 + chip2 + chip3 + chip4 + chip5 + chip6;
            sut.GameLogicManager.CreateQueue(players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            SetupClass.SetNewHoleCards(sb, new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(bb, new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack), new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(none3, new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two), new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var joker = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(joker.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity> { new(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), }, out var isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out var isGameOver, out var isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void ThreePlayersUseHandInfluenceJokerTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6)
        {
            var sut = new SetupClass.TestSystem();
            var p1 = SetupClass.SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
            var p2 = SetupClass.SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
            var p3 = SetupClass.SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p4 = SetupClass.SetupTestHand("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p5 = SetupClass.SetupTestHand("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p6 = SetupClass.SetupTestHand("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);

            var players = new List<PlayerEntity> { p1, p2, p3, p4, p5, p6 };
            sut.GameLogicManager.SetupGame(players, true);
            var sb = players.First(x => x.Name == "small");
            sb.IsDealer = false;
            sb.PlayerRole = role1;
            sb.Chips = chip1;

            var bb = players.First(x => x.Name == "big");
            bb.IsDealer = false;
            bb.PlayerRole = role2;
            bb.Chips = chip2;

            var none3 = players.First(x => x.Name == "none3");
            none3.IsDealer = false;
            none3.PlayerRole = role3;
            none3.Chips = chip3;

            var none4 = players.First(x => x.Name == "none4");
            none4.IsDealer = false;
            none4.PlayerRole = role4;
            none4.Chips = chip4;

            var none5 = players.First(x => x.Name == "none5");
            none5.IsDealer = false;
            none5.PlayerRole = role5;
            none5.Chips = chip5;

            var none6 = players.First(x => x.Name == "none6");
            none6.IsDealer = true;
            none6.PlayerRole = role6;
            none6.Chips = chip6;

            var totalChips = chip1 + chip2 + chip3 + chip4 + chip5 + chip6;
            sut.GameLogicManager.CreateQueue(players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            SetupClass.SetNewHoleCards(sb, new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(bb, new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack), new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(none3, new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two), new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var jokerEntity = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity>() { new(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), }, out var isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out var isGameOver, out var isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            var jokerEntity2 = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity2.JokerId, none4, out _, out addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, none4, [none4], none4.JokerCards.First(), new List<CardEntity>() { none4.HoleCards.First() }, out isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            var jokerEntity3 = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity3.JokerId, none5, out _, out addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, none5, [none5], none5.JokerCards.First(), new List<CardEntity>() { none5.HoleCards.First() }, out isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isError);
            sut.JokerManager.UseJoker(sut.GameLogicManager, none5, [none5], none5.JokerCards.First(), new List<CardEntity>() { none5.HoleCards.First() }, out isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isError);
            sut.JokerManager.UseJoker(sut.GameLogicManager, none5, [none5], none5.JokerCards.First(), new List<CardEntity>() { none5.HoleCards.First() }, out isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void PlayerUsesActionInfluenceJokerToForceRaiseTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6)
        {
            var sut = new SetupClass.TestSystem();
            var p1 = SetupClass.SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
            var p2 = SetupClass.SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
            var p3 = SetupClass.SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p4 = SetupClass.SetupTestHand("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p5 = SetupClass.SetupTestHand("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p6 = SetupClass.SetupTestHand("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);

            var players = new List<PlayerEntity> { p1, p2, p3, p4, p5, p6 };
            sut.GameLogicManager.SetupGame(players, true);
            var sb = players.First(x => x.Name == "small");
            sb.IsDealer = false;
            sb.PlayerRole = role1;
            sb.Chips = chip1;

            var bb = players.First(x => x.Name == "big");
            bb.IsDealer = false;
            bb.PlayerRole = role2;
            bb.Chips = chip2;

            var none3 = players.First(x => x.Name == "none3");
            none3.IsDealer = false;
            none3.PlayerRole = role3;
            none3.Chips = chip3;

            var none4 = players.First(x => x.Name == "none4");
            none4.IsDealer = false;
            none4.PlayerRole = role4;
            none4.Chips = chip4;

            var none5 = players.First(x => x.Name == "none5");
            none5.IsDealer = false;
            none5.PlayerRole = role5;
            none5.Chips = chip5;

            var none6 = players.First(x => x.Name == "none6");
            none6.IsDealer = true;
            none6.PlayerRole = role6;
            none6.Chips = chip6;

            var totalChips = chip1 + chip2 + chip3 + chip4 + chip5 + chip6;
            sut.GameLogicManager.CreateQueue(players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            SetupClass.SetNewHoleCards(sb, new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(bb, new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack), new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(none3, new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two), new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            //force raise
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 103);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p2], p1.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out var isGameOver, out var isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, true, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, true, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400)]
        public void PlayerUsesActionInfluenceJokerToPreventCheckTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = new SetupClass.TestSystem();
            var p1 = SetupClass.SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
            var p2 = SetupClass.SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
            var p3 = SetupClass.SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
            var p4 = SetupClass.SetupTestHand("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);

            var players = new List<PlayerEntity> { p1, p2, p3, p4 };
            sut.GameLogicManager.SetupGame(players, true);
            var sb = players.First(x => x.Name == "small");
            sb.IsDealer = false;
            sb.PlayerRole = role1;
            sb.Chips = chip1;

            var bb = players.First(x => x.Name == "big");
            bb.IsDealer = false;
            bb.PlayerRole = role2;
            bb.Chips = chip2;

            var none3 = players.First(x => x.Name == "none3");
            none3.IsDealer = false;
            none3.PlayerRole = role3;
            none3.Chips = chip3;

            var none4 = players.First(x => x.Name == "none4");
            none4.IsDealer = true;
            none4.PlayerRole = role4;
            none4.Chips = chip4;

            var totalChips = chip1 + chip2 + chip3 + chip4;
            sut.GameLogicManager.CreateQueue(players);

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

            SetupClass.SetNewHoleCards(sb, new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(bb, new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack), new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine));
            SetupClass.SetNewHoleCards(none3, new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two), new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five));

            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            //prevent check
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 103);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p2], p1.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out var isGameOver, out var isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, true, isError, false, isGameOver);
            sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
            SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, isGameOver);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
    }
}