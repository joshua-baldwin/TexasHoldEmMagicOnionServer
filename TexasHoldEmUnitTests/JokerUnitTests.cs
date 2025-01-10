using THE.Shared.Enums;
using THE.Entities;
using THE.Shared.Utilities;

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
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            
            var joker = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(joker.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity> { new(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), }, out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void PlayerUsesHandInfluenceJokerToDrawThenDiscardTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            
            var joker = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 102);
            sut.JokerManager.PurchaseJoker(joker.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity> { new(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), }, out var isJokerError, out _, out _);
            sut.GameLogicManager.DiscardAndFinishUsingJoker(p1, p1.JokerCards.First(), [p1.HoleCards.First()]);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
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
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];
            var p5 = sut.Players[4];
            
            var jokerEntity = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity>() { new(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King), }, out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            var jokerEntity2 = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity2.JokerId, p4, out _, out addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p4, [p4], p4.JokerCards.First(), new List<CardEntity>() { p4.HoleCards.First() }, out isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            var jokerEntity3 = sut.JokerManager.GetJokerEntities().First();
            sut.JokerManager.PurchaseJoker(jokerEntity3.JokerId, p5, out _, out addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p5, [p5], p5.JokerCards.First(), new List<CardEntity>() { p5.HoleCards.First() }, out isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.JokerManager.UseJoker(sut.GameLogicManager, p5, [p5], p5.JokerCards.First(), new List<CardEntity>() { p5.HoleCards.First() }, out isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            sut.JokerManager.UseJoker(sut.GameLogicManager, p5, [p5], p5.JokerCards.First(), new List<CardEntity>() { p5.HoleCards.First() }, out isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
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
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            //force raise
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 104);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p2], p1.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400)]
        public void PlayerUsesActionInfluenceJokerToPreventCheckTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            //prevent check
            SetupClass.PurchaseAndUseJoker(sut, 104, p1, [p2], [], ref totalChips);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void CantForceAndPreventTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            //force raise
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 104);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p1, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p3], p1.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, false, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            
            var jokerEntity2 = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 105);
            sut.JokerManager.PurchaseJoker(jokerEntity2.JokerId, p2, out _, out addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p2, [p3], p2.JokerCards.First(), new List<CardEntity>(), out isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, true, isJokerError);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400)]
        public void PlayerUsesActionInfluenceJokerToChangePositionTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            //change position
            SetupClass.PurchaseAndUseJoker(sut, 106, p1, [p1], [], ref totalChips);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none4"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none4"));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            
            SetupClass.PurchaseAndUseJoker(sut, 106, p3, [p3], [], ref totalChips);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none4"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 400,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400, Enums.PlayerRoleEnum.None, 400)]
        public void PlayerUsesActionInfluenceJokerToChangePositionWithAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            //change position
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            
            SetupClass.PurchaseAndUseJoker(sut, 106, p3, [p3], [], ref totalChips);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none4"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none5"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none4"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none5"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600, Enums.PlayerRoleEnum.None, 350)]
        public void ChangePositionBackToBackTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            //change position
            SetupClass.PurchaseAndUseJoker(sut, 106, p1, [p1], [], ref totalChips);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            
            SetupClass.PurchaseAndUseJoker(sut, 106, p2, [p2], [], ref totalChips);
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("small"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("big"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetCurrentPlayer().Name, Is.EqualTo("none3"));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600, Enums.PlayerRoleEnum.None, 350)]
        public void IncreaseBettingRoundTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            //change position
            SetupClass.PurchaseAndUseJoker(sut, 108, p1, [p1], [], ref totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            Assert.That(sut.GameLogicManager.GetCurrentExtraBettingRound(), Is.EqualTo(1));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600, Enums.PlayerRoleEnum.None, 350)]
        public void CantIncreaseBettingRoundTwiceTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            //change position
            SetupClass.PurchaseAndUseJoker(sut, 108, p1, [p1], [], ref totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            
            sut.JokerManager.UseJoker(sut.GameLogicManager, p1, [p1], p1.JokerCards.First(), new List<CardEntity>(), out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, true, isJokerError);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            Assert.That(sut.GameLogicManager.GetCurrentExtraBettingRound(), Is.EqualTo(1));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600, Enums.PlayerRoleEnum.None, 350)]
        public void ChooseCardFromPoolTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.AssertAfterAction(sut, totalChips, false, false, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            SetupClass.PurchaseAndUseJoker(sut, 103, p1, [p1], [new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace)], ref totalChips);
            Assert.That(p1.HoleCards.Count, Is.EqualTo(3));
            Assert.That(p1.HoleCards.FirstOrDefault(x => x.Rank == Enums.CardRankEnum.Ace && x.Suit == Enums.CardSuitEnum.Spade) != null);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600, Enums.PlayerRoleEnum.None, 350)]
        public void CantUseJokerTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            SetupClass.PurchaseAndUseJoker(sut, 109, p1, [], [], ref totalChips);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            
            var jokerEntity = sut.JokerManager.GetJokerEntities().First(x => x.JokerId == 104);
            sut.JokerManager.PurchaseJoker(jokerEntity.JokerId, p2, out _, out var addedJoker);
            totalChips -= addedJoker.BuyCost;
            sut.JokerManager.UseJoker(sut.GameLogicManager, p2, [p1], p2.JokerCards.First(), [], out var isJokerError, out _, out _);
            SetupClass.AssertAfterJokerAction(sut, totalChips, true, isJokerError);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100)]
        public void CantUseCommunityCardTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            //king nine
            //ten nine
            sut.GameLogicManager.SetCommunityCardsForUnitTests(new List<CardEntity>()
            {
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two),
            });
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            SetupClass.PurchaseAndUseJoker(sut, 113, p1, [], [new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten)], ref totalChips);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            var winners = sut.GameLogicManager.DoShowdown();
            Assert.That(winners.First().Winner.Id, Is.EqualTo(p1.Id));
        }
    }
}