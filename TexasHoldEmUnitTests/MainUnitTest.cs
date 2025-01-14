using MagicOnion.Server.Hubs;
using THE.GameLogic;
using THE.Shared.Enums;
using THE.Entities;
using THE.Managers;
using THE.Shared.Utilities;

namespace TexasHoldEmUnitTests
{
    public partial class Tests
    {
        #region Ranking tests

        [Test]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.King,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ace,
            Enums.HandRankingType.RoyalFlush)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack,
            Enums.HandRankingType.StraightFlush)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Seven,
            Enums.HandRankingType.FourOfAKind)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Six,
            Enums.HandRankingType.FullHouse)]
        [TestCase(
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Seven,
            Enums.HandRankingType.Flush)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.HandRankingType.Straight)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.HandRankingType.ThreeOfAKind)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.HandRankingType.TwoPair)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.HandRankingType.Pair)]
        [TestCase(
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three,
            Enums.HandRankingType.HighCard)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine,
            Enums.HandRankingType.HighCard)]
        public void HandIsRanking(
            Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank,
            Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank,
            Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank,
            Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank,
            Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank,
            Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank,
            Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank,
            Enums.HandRankingType handRankingType)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("1", Enums.PlayerRoleEnum.None, holeCard1Suit, holeCard1Rank, holeCard2Suit, holeCard2Rank, 10)
            ], true, out _);
            
            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(communityCard1Suit, communityCard1Rank),
                new CardEntity(communityCard2Suit, communityCard2Rank),
                new CardEntity(communityCard3Suit, communityCard3Rank),
                new CardEntity(communityCard4Suit, communityCard4Rank),
                new CardEntity(communityCard5Suit, communityCard5Rank)
            ]);
            var ranking = HandRankingLogic.GetHandRanking(sut.GetHand(sut.Players[0]));
            Assert.That(ranking, Is.EqualTo(handRankingType));
        }

        #endregion

        #region Compare tests

        [Test]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.StraightFlush)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.FourOfAKind)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.HandRankingType.FourOfAKind)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.FourOfAKind)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine,
            Enums.HandRankingType.FullHouse)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.FullHouse)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.Flush)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.King,
            Enums.HandRankingType.Straight)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.ThreeOfAKind)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Queen,
            Enums.HandRankingType.TwoPair)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Queen,
            Enums.HandRankingType.Pair)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Queen,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Four,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine,
            Enums.HandRankingType.HighCard)]
        public void CompareHandTest(Enums.CardSuitEnum player1HoleCard1Suit, Enums.CardRankEnum player1HoleCard1Rank,
            Enums.CardSuitEnum player1HoleCard2Suit, Enums.CardRankEnum player1HoleCard2Rank,
            Enums.CardSuitEnum player2HoleCard1Suit, Enums.CardRankEnum player2HoleCard1Rank,
            Enums.CardSuitEnum player2HoleCard2Suit, Enums.CardRankEnum player2HoleCard2Rank,
            Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank,
            Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank,
            Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank,
            Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank,
            Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank,
            Enums.HandRankingType handRankingType)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("1", Enums.PlayerRoleEnum.None, player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, 10),
                ("2", Enums.PlayerRoleEnum.None, player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, 10),
            ], true, out _);
            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(communityCard1Suit, communityCard1Rank),
                new CardEntity(communityCard2Suit, communityCard2Rank),
                new CardEntity(communityCard3Suit, communityCard3Rank),
                new CardEntity(communityCard4Suit, communityCard4Rank),
                new CardEntity(communityCard5Suit, communityCard5Rank)
            ]);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            var p1Hand = sut.GetHand(p1);
            var p2Hand = sut.GetHand(p2);
            var ranking1 = HandRankingLogic.GetHandRanking(p1Hand);
            var ranking2 = HandRankingLogic.GetHandRanking(p2Hand);
            var winner = HandRankingLogic.CompareHands((p1.Id, p1Hand.Where(x => x.IsFinalHand).ToList()), (p2.Id, p2Hand.Where(x => x.IsFinalHand).ToList()), handRankingType);

            Assert.That(ranking1, Is.EqualTo(handRankingType));
            Assert.That(ranking2, Is.EqualTo(handRankingType));
            Assert.That(p1Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
            Assert.That(p2Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
            Assert.That(p2.Id, Is.EqualTo(winner));
        }

        #endregion

        #region Tie tests

        [Test]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Six,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.FullHouse)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten,
            Enums.HandRankingType.ThreeOfAKind)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five,
            Enums.HandRankingType.TwoPair)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Five,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ace,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Queen,
            Enums.HandRankingType.Pair)]
        [TestCase(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.King,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Club, Enums.CardRankEnum.King,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Jack,
            Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Three,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
            Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Two,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten,
            Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Queen,
            Enums.HandRankingType.HighCard)]
        public void ComparingHandsShouldBeTie(Enums.CardSuitEnum player1HoleCard1Suit, Enums.CardRankEnum player1HoleCard1Rank,
            Enums.CardSuitEnum player1HoleCard2Suit, Enums.CardRankEnum player1HoleCard2Rank,
            Enums.CardSuitEnum player2HoleCard1Suit, Enums.CardRankEnum player2HoleCard1Rank,
            Enums.CardSuitEnum player2HoleCard2Suit, Enums.CardRankEnum player2HoleCard2Rank,
            Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank,
            Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank,
            Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank,
            Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank,
            Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank,
            Enums.HandRankingType handRankingType)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("1", Enums.PlayerRoleEnum.None, player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, 20),
                ("2", Enums.PlayerRoleEnum.None, player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, 20),
            ], true, out _);
            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(communityCard1Suit, communityCard1Rank),
                new CardEntity(communityCard2Suit, communityCard2Rank),
                new CardEntity(communityCard3Suit, communityCard3Rank),
                new CardEntity(communityCard4Suit, communityCard4Rank),
                new CardEntity(communityCard5Suit, communityCard5Rank)
            ]);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            
            var p1Hand = sut.GetHand(p1);
            var p2Hand = sut.GetHand(p2);
            var ranking1 = HandRankingLogic.GetHandRanking(p1Hand);
            var ranking2 = HandRankingLogic.GetHandRanking(p2Hand);
            var winner = HandRankingLogic.CompareHands((p1.Id, p1Hand.Where(x => x.IsFinalHand).ToList()), (p2.Id, p2Hand.Where(x => x.IsFinalHand).ToList()), handRankingType);

            Assert.That(ranking1, Is.EqualTo(handRankingType));
            Assert.That(ranking2, Is.EqualTo(handRankingType));
            Assert.That(p1Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
            Assert.That(p2Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
            Assert.That(winner, Is.EqualTo(Guid.Empty));
        }

        #endregion

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 20)]
        public void GameStateTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip3),
            ], true, out var totalChips);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);


            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);

            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 60, Enums.PlayerRoleEnum.BigBlind, 80, Enums.PlayerRoleEnum.None, 20)]
        public void RaiseTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 10, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.GameLogicManager.GetPreviousPlayer().CurrentBet, Is.EqualTo(15));
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 20, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.GameLogicManager.GetPreviousPlayer().CurrentBet, Is.EqualTo(35));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 40, Enums.PlayerRoleEnum.BigBlind, 80)]
        public void AllInWith2Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var mainPot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + mainPot));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 10)]
        public void AllInWith3PlayersAndLastTwoGoAllIn(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var toReturn = sut.GameLogicManager.GetPots()[0].PotAmount;
            var sidePot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[2].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(toReturn));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(sidePot));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));

            Assert.That(winners[2].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[2].PotToWinner, Is.EqualTo(mainPot));
            Assert.That(winners[2].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + toReturn));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + sidePot + mainPot));
            Assert.That(p3.Chips, Is.EqualTo(0));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 60)]
        public void AllInWith3PlayersAndFirstTwoGoAllIn(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            var sbBet = p1.CurrentBet;
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            var p3ChipsBeforeBet = p3.Chips;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBeforeBet - sbBet));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 50)]
        public void AllInWith3PlayersAndLastCannotCallSoTheyGoAllIn(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            var sbBet = p1.CurrentBet;
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            var p3ChipsBeforeBet = p3.Chips;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBeforeBet - sbBet));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 10)]
        public void AllInWith3PlayersAndFirstTwoGoAllInLastFolds(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 70, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80)]
        public void AllInWith3PlayersAllInThenCallThenRaise(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            var prevBet = sut.GameLogicManager.GetPreviousPlayer().CurrentBet;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.Players[2].CurrentBet, Is.EqualTo(prevBet));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 40, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 70, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80)]
        public void AllInWith3PlayersAllInThenCallThenCall(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            var prevBet = sut.GameLogicManager.GetPreviousPlayer().CurrentBet;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.Players[2].CurrentBet, Is.EqualTo(prevBet));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 7, Enums.PlayerRoleEnum.None, 30)]
        public void AllInWith3PlayersAllInThenCallWithoutReRaise(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            var preFlopTotal = sut.GameLogicManager.GetPots()[0].PotAmount;

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            var p1PrevBet = sut.GameLogicManager.GetPreviousPlayer().CurrentBet;
            var allInBet = sut.GameLogicManager.GetCurrentPlayer().Chips;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            Assert.That(sut.GameLogicManager.GetPots().Count, Is.EqualTo(2));
            Assert.That(sut.GameLogicManager.GetPots()[1].PotAmount, Is.EqualTo(preFlopTotal + (p1PrevBet * 2) + allInBet));

            var raiseAmount = 5;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, raiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            

            var currentSidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var currentMainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            Assert.That(currentSidePot, Is.EqualTo(raiseAmount * 2));
            Assert.That(currentMainPot, Is.EqualTo(preFlopTotal + p1PrevBet * sut.Players.Count));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 13, Enums.PlayerRoleEnum.None, 30, 10)]
        public void AllInWith3PlayersAllInThenCallWithReRaise(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, int raiseAmount)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            var preFlopTotal = sut.GameLogicManager.GetPots()[0].PotAmount;

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            var p2PrevBet = sut.GameLogicManager.GetPreviousPlayer().CurrentBet;
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.Players[2].CurrentBet, Is.EqualTo(p2PrevBet));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            var currentSidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var currentMainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            Assert.That(currentSidePot, Is.EqualTo(0));
            Assert.That(currentMainPot, Is.EqualTo(preFlopTotal + p2PrevBet * sut.Players.Count));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 90, Enums.PlayerRoleEnum.BigBlind, 17, Enums.PlayerRoleEnum.None, 150, 40, false, Enums.GameStateEnum.TheTurn)]
        public void AllInWith3PlayersRaiseAfterAllIn(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, int raiseAmount, bool isBetError, Enums.GameStateEnum newGameState)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            var initialRaise = 5;
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, initialRaise, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 20, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.GameLogicManager.GetPreviousPlayer().CurrentBet, Is.EqualTo(35));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 40, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            Assert.That(sut.GameLogicManager.GetPreviousPlayer().CurrentBet, Is.EqualTo(75));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            var val = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            var val2 = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val2));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 90, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80, false, Enums.GameStateEnum.TheTurn)]
        public void AllInWith3PlayersReRaiseAfterAllInThenFold(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, bool isBetError, Enums.GameStateEnum newGameState)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.Players[2].CurrentBet, Is.EqualTo(chip2 - Constants.MinBet));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 40, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            
            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p3.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + mainPot));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore + sidePot));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 10, Enums.PlayerRoleEnum.None, 50)]
        public void AllInWith4Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);


            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var none2ChipsBefore = p4.Chips;
            var sidePotA = sut.GameLogicManager.GetPots()[0].PotAmount;
            var sidePotB = sut.GameLogicManager.GetPots()[1].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[2].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p4.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePotA));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(sidePotB));

            Assert.That(winners[2].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[2].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner + winners[2].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(0));
            Assert.That(p4.Chips, Is.EqualTo(none2ChipsBefore + winners[0].PotToWinner));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.Straight));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 89, Enums.PlayerRoleEnum.None, 42, Enums.PlayerRoleEnum.None, 42)]
        public void AllInWith4PlayersShouldHave2PotsWithSomethingInThem(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);


            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 50, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 10, Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 38, Enums.PlayerRoleEnum.None, 50)]
        public void AllInWith5Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen, chip4),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace, chip5),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];
            var p5 = sut.Players[4];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var none1ChipsBefore = p3.Chips;
            var none2ChipsBefore = p4.Chips;
            var none3ChipsBefore = p5.Chips;
            var sidePotA = sut.GameLogicManager.GetPots()[0].PotAmount;
            var sidePotB = sut.GameLogicManager.GetPots()[1].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[2].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePotA));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p4.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(sidePotB));

            Assert.That(winners[2].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[2].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[2].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(none1ChipsBefore));
            Assert.That(p4.Chips, Is.EqualTo(none2ChipsBefore + winners[1].PotToWinner));
            Assert.That(p5.Chips, Is.EqualTo(none3ChipsBefore));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.Straight));
            Assert.That(winners[2].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 10, Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 37, Enums.PlayerRoleEnum.None, 50)]
        public void AllInWith5PlayersStaggered(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen, chip4),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace, chip5),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 35, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots().Count, Is.EqualTo(3));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            var val = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));
            var val2 = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val2));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];
            var p5 = sut.Players[4];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var none1ChipsBefore = p3.Chips;
            var none2ChipsBefore = p4.Chips;
            var none3ChipsBefore = p5.Chips;
            var sidePotA = sut.GameLogicManager.GetPots()[0].PotAmount;
            var sidePotB = sut.GameLogicManager.GetPots()[1].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[2].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePotA));

            Assert.That(winners[1].Winner.Id, Is.EqualTo(p4.Id));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(sidePotB));

            Assert.That(winners[2].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[2].PotToWinner, Is.EqualTo(mainPot));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[2].PotToWinner));
            Assert.That(p3.Chips, Is.EqualTo(none1ChipsBefore));
            Assert.That(p4.Chips, Is.EqualTo(none2ChipsBefore + winners[1].PotToWinner));
            Assert.That(p5.Chips, Is.EqualTo(none3ChipsBefore));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.Straight));
            Assert.That(winners[2].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30)]
        public void TiePotTestWith3Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.King, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
             var p1 = sut.Players[0];
            var p2 = sut.Players[1];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var mainPot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].TiedWith.Select(x => x.Id), Has.Member(p1.Id));
            Assert.That(winners[0].TiedWith.Select(x => x.Id), Has.Member(p2.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(0));
            Assert.That(winners[0].PotToTiedWith, Is.EqualTo(mainPot / 2));

            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToTiedWith));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[0].PotToTiedWith));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30)]
        public void EveryoneFoldTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.King, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, true);

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var mainPot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(mainPot));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.Nothing));
            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + mainPot));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.GameOver));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 32, Enums.PlayerRoleEnum.None, 32)]
        public void EveryoneFoldAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
            var val = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 5, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            var val2 = sut.GameLogicManager.GetPlayerQueue().First().CurrentBet;
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == val2));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);
            
            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];

            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var p4ChipsBefore = p4.Chips;
            var sidePot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var mainPot = sut.GameLogicManager.GetPots()[1].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
            Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));
            Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
            Assert.That(winners[1].HandRanking, Is.EqualTo(Enums.HandRankingType.FullHouse));
            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + sidePot));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + mainPot));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(p4.Chips, Is.EqualTo(p4ChipsBefore));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30, Enums.PlayerRoleEnum.None, 30)]
        public void FoldDuringPreFlopTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, true);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.GameOver));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            ]);

            var p1 = sut.Players[0];
            var p2 = sut.Players[1];
            var p3 = sut.Players[2];
            var p4 = sut.Players[3];
            
            var sbChipsBefore = p1.Chips;
            var bbChipsBefore = p2.Chips;
            var p3ChipsBefore = p3.Chips;
            var p4ChipsBefore = p4.Chips;
            var mainPot = sut.GameLogicManager.GetPots()[0].PotAmount;
            var winners = sut.GameLogicManager.DoShowdown();

            Assert.That(winners[0].Winner.Id, Is.EqualTo(p2.Id));
            Assert.That(mainPot, Is.EqualTo(Constants.MinBet / 2 + Constants.MinBet));
            Assert.That(winners[0].PotToWinner, Is.EqualTo(mainPot));
            Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.Nothing));
            Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore));
            Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + mainPot));
            Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
            Assert.That(p4.Chips, Is.EqualTo(p4ChipsBefore));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30, Enums.PlayerRoleEnum.None, 30)]
        public void FinalBetsAreEqualTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPlayerQueue().All(x => x.CurrentBet == Constants.MinBet));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 25, Enums.PlayerRoleEnum.None, 30)]
        public void GameShouldEndAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 2, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 25, Enums.PlayerRoleEnum.None, 30)]
        public void ThreeWayTieTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
            
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight)
            ]);

            var winners = sut.GameLogicManager.DoShowdown();
            Assert.That(winners.First().TiedWith.Count, Is.EqualTo(3));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 80, Enums.PlayerRoleEnum.None, 60)]
        public void RaiseThenEveryoneAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 60, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].PotAmount, Is.EqualTo(3));
            Assert.That(sut.GameLogicManager.GetPots()[1].PotAmount, Is.EqualTo(123));
            Assert.That(sut.GameLogicManager.GetPots().All(x => x.PotAmount >= 0), Is.True);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].PotAmount, Is.EqualTo(5));
            Assert.That(sut.GameLogicManager.GetPots()[1].PotAmount, Is.EqualTo(181));
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 2, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].PotAmount, Is.EqualTo(42));
            Assert.That(sut.GameLogicManager.GetPots()[1].PotAmount, Is.EqualTo(37));
            Assert.That(sut.GameLogicManager.GetPots()[2].PotAmount, Is.EqualTo(181));

            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));

            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight)
            ]);

            var winners = sut.GameLogicManager.DoShowdown();
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 10, Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 38, Enums.PlayerRoleEnum.None, 50)]
        public void RemoveFromPotAfterFoldTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen, chip4),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace, chip5),
                ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 25, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[1].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[1].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        public void PullFromCorrectPotAfterMultipleAllInsTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6,
            Enums.PlayerRoleEnum role7, int chip7, Enums.PlayerRoleEnum role8, int chip8,
            Enums.PlayerRoleEnum role9, int chip9, Enums.PlayerRoleEnum role10, int chip10)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
                ("none7", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip7),
                ("none8", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip8),
                ("none9", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip9),
                ("none10", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip10),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        public void TooManyChipsTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6,
            Enums.PlayerRoleEnum role7, int chip7, Enums.PlayerRoleEnum role8, int chip8,
            Enums.PlayerRoleEnum role9, int chip9, Enums.PlayerRoleEnum role10, int chip10)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
                ("none7", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip7),
                ("none8", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip8),
                ("none9", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip9),
                ("none10", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip10),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetPots()[0].EligiblePlayers.Count, Is.EqualTo(sut.GameLogicManager.GetPlayerQueue().Count(x => !x.HasFolded)));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        public void TenAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6,
            Enums.PlayerRoleEnum role7, int chip7, Enums.PlayerRoleEnum role8, int chip8,
            Enums.PlayerRoleEnum role9, int chip9, Enums.PlayerRoleEnum role10, int chip10)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
                ("none7", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip7),
                ("none8", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip8),
                ("none9", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip9),
                ("none10", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip10),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 3000, Enums.PlayerRoleEnum.BigBlind, 3000,
            Enums.PlayerRoleEnum.None, 2400, Enums.PlayerRoleEnum.None, 2500,
            Enums.PlayerRoleEnum.None, 1800, Enums.PlayerRoleEnum.None, 1400,
            Enums.PlayerRoleEnum.None, 801, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 201, Enums.PlayerRoleEnum.None, 100)]
        public void TestTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4,
            Enums.PlayerRoleEnum role5, int chip5, Enums.PlayerRoleEnum role6, int chip6,
            Enums.PlayerRoleEnum role7, int chip7, Enums.PlayerRoleEnum role8, int chip8,
            Enums.PlayerRoleEnum role9, int chip9, Enums.PlayerRoleEnum role10, int chip10)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
                ("none5", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip5),
                ("none6", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip6),
                ("none7", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip7),
                ("none8", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip8),
                ("none9", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip9),
                ("none10", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip10),
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

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheRiver));

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        //raise amount wasnt getting added to the chipamountbeforeallin
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 25, Enums.PlayerRoleEnum.None, 30)]
        public void RaiseAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 2, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 55, Enums.PlayerRoleEnum.None, 30, Enums.PlayerRoleEnum.None, 101)]
        public void AllInThenRaiseThenAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);

            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 2, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }

        //after round ended i wasnt locking all pots that were all-in-pots
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 600, Enums.PlayerRoleEnum.BigBlind, 600,
            Enums.PlayerRoleEnum.None, 350, Enums.PlayerRoleEnum.None, 400,
            Enums.PlayerRoleEnum.None, 150, Enums.PlayerRoleEnum.None, 200)]
        public void RaiseAndCallAfterMultipleAllInsTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
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
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100, Enums.PlayerRoleEnum.None, 100)]
        public void BigBlindCanCheckTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100, Enums.PlayerRoleEnum.None, 100)]
        public void BigBlindCantCheckAfterRaiseTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        public void BigBlindCantCheckAfterRaiseCallFoldTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4)
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 30, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 1)]
        public void BigBlindCanCheckAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4)
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 30, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 5)]
        public void BigBlindCantCheckAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 30, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 5)]
        public void CanCheckIfFirstTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 30, Enums.PlayerRoleEnum.BigBlind, 100, Enums.PlayerRoleEnum.None, 100)]
        public void ResetMaxHoleCardsAfterRoundTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, true);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.GameOver));
            
            SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], false, out totalChips, sut);
            
            SetupClass.AssertBeforeAction(sut);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertBeforeAction(sut);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.AssertBeforeAction(sut);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.AssertBeforeAction(sut);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, false);
            SetupClass.AssertBeforeAction(sut);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Fold, 0, totalChips, false, true);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.GameOver));
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100)]
        public void WinTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Seven, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Two, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Seven, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ten, chip2),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
            
            sut.GameLogicManager.SetCommunityCardsForUnitTests([
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Three),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Three),
                new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight),
                new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace)
            ]);
            
            var winners = sut.GameLogicManager.DoShowdown();
            Assert.That(winners.Count, Is.EqualTo(1));
            Assert.That(winners.First().TiedWith.Count, Is.EqualTo(2));
        }

        [Test]
        public void JoinRoomTest()
        {
            var playerList = new DefaultInMemoryStorage<PlayerEntity>();
            var roomManager = new RoomManager();
            var self = new PlayerEntity("p1", Guid.NewGuid(), Enums.PlayerRoleEnum.None);
            self.Chips = Constants.StartingChips;
            
            var roomId = Guid.NewGuid();
            self.RoomId = roomId;
            playerList.Set(self.Id, self);;
            roomManager.AddRoomAndConnection(roomId, playerList, self.Id, Guid.NewGuid(), new GameLogicManager(), new JokerManager());
            
            var existingRoom = roomManager.GetNonFullRoomEntity();
            for (var i = 0; i < 9; i++)
            {
                Assert.That(existingRoom, Is.Not.Null);
                self = new PlayerEntity($"p{i + 2}", Guid.NewGuid(), Enums.PlayerRoleEnum.None);
                self.Chips = Constants.StartingChips;
                self.RoomId = roomId;
                playerList.Set(self.Id, self);
                roomManager.AddConnection(existingRoom.Id, self.Id, Guid.NewGuid());
            }
            existingRoom = roomManager.GetNonFullRoomEntity();
            Assert.That(existingRoom, Is.Null);
        }
        
        // [Test]
        // [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 1,
        //     Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        // public void BigBlindGoesAllInDuringBlindBetTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
        //     Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        // {
        //     var playersToCreate = new List<(PlayerEntity, Enums.CardSuitEnum, Enums.CardRankEnum, Enums.CardSuitEnum, Enums.CardRankEnum, int)> {
        //         (new PlayerEntity("small", Guid.NewGuid(), Enums.PlayerRoleEnum.SmallBlind), Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
        //         (new PlayerEntity("big", Guid.NewGuid(), Enums.PlayerRoleEnum.BigBlind), Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
        //         (new PlayerEntity("none3", Guid.NewGuid(), Enums.PlayerRoleEnum.None), Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
        //         (new PlayerEntity("none4", Guid.NewGuid(), Enums.PlayerRoleEnum.None), Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
        //     };
        //     
        //     var players = new List<PlayerEntity>();
        //     var sut = new SetupClass.TestSystem();
        //     foreach (var player in playersToCreate)
        //     {
        //         var p = SetupClass.SetupTestHand(player.Item1.Name, player.Item1.PlayerRole);
        //         players.Add(p);
        //     }
        //
        //     sut.GameLogicManager.SetupGame(players, true);
        //     sut.Players = sut.GameLogicManager.GetAllPlayers();
        //
        //     for (var i = 0; i < sut.Players.Count; i++)
        //     {
        //         var player = sut.Players[i];
        //         player.IsDealer = i == sut.Players.Count - 1;
        //         player.PlayerRole = playersToCreate[i].Item1.PlayerRole;
        //         player.Chips = playersToCreate[i].Item6;
        //     }
        //
        //     var totalChips = sut.Players.Select(x => x.Chips).Sum();
        //     sut.GameLogicManager.CreateQueue(sut.Players);
        //
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.SmallBlindBet, 0, totalChips, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.BigBlindBet, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, true, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.PreFlop));
        //
        //     for (var i = 0; i < sut.Players.Count; i++)
        //         SetupClass.SetNewHoleCards(sut.Players[i], [new CardEntity(playersToCreate[i].Item2, playersToCreate[i].Item3), new CardEntity(playersToCreate[i].Item4, playersToCreate[i].Item5)]);
        //     
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        //     
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        //     Assert.That(sut.GameLogicManager.GetPots().Count, Is.EqualTo(2));
        // }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100)]
        public void CallAfterBigBlindRaisesDuringPreFlopTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 30, Enums.PlayerRoleEnum.BigBlind, 100,
            Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        public void CallAfterBigBlindRaisesDuringPreFlopSecondTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
            Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
                ("none4", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, 1, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        }
        
        // [Test]
        // [TestCase(Enums.PlayerRoleEnum.SmallBlind, 1, Enums.PlayerRoleEnum.BigBlind, 100,
        //     Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 100)]
        // public void SmallBlindGoesAllInDuringPreFlopTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2,
        //     Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
        // {
        //     var playersToCreate = new List<(PlayerEntity, Enums.CardSuitEnum, Enums.CardRankEnum, Enums.CardSuitEnum, Enums.CardRankEnum, int)> {
        //         (new PlayerEntity("small", Guid.NewGuid(), Enums.PlayerRoleEnum.SmallBlind), Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
        //         (new PlayerEntity("big", Guid.NewGuid(), Enums.PlayerRoleEnum.BigBlind), Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
        //         (new PlayerEntity("none3", Guid.NewGuid(), Enums.PlayerRoleEnum.None), Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
        //         (new PlayerEntity("none4", Guid.NewGuid(), Enums.PlayerRoleEnum.None), Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip4),
        //     };
        //     
        //     var players = new List<PlayerEntity>();
        //     var sut = new SetupClass.TestSystem();
        //     foreach (var player in playersToCreate)
        //     {
        //         var p = SetupClass.SetupTestHand(player.Item1.Name, player.Item1.PlayerRole);
        //         players.Add(p);
        //     }
        //
        //     sut.GameLogicManager.SetupGame(players, true);
        //     sut.Players = sut.GameLogicManager.GetAllPlayers();
        //
        //     for (var i = 0; i < sut.Players.Count; i++)
        //     {
        //         var player = sut.Players[i];
        //         player.IsDealer = i == sut.Players.Count - 1;
        //         player.PlayerRole = playersToCreate[i].Item1.PlayerRole;
        //         player.Chips = playersToCreate[i].Item6;
        //     }
        //
        //     var totalChips = sut.Players.Select(x => x.Chips).Sum();
        //     sut.GameLogicManager.CreateQueue(sut.Players);
        //
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.SmallBlindBet, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.BigBlindBet, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //
        //     for (var i = 0; i < sut.Players.Count; i++)
        //         SetupClass.SetNewHoleCards(sut.Players[i], [new CardEntity(playersToCreate[i].Item2, playersToCreate[i].Item3), new CardEntity(playersToCreate[i].Item4, playersToCreate[i].Item5)]);
        //     
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, true, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheFlop));
        //     
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Raise, Constants.RaiseAmount, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
        //     SetupClass.AssertAfterAction(sut, totalChips, false, isError, false, false);
        //     Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.TheTurn));
        //     Assert.That(sut.GameLogicManager.GetPots().Count, Is.EqualTo(2));
        // }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100, Enums.PlayerRoleEnum.None, 1)]
        public void CanCheckAfterAllInDuringPreFlopTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
            
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 2, Enums.PlayerRoleEnum.BigBlind, 100, Enums.PlayerRoleEnum.None, 100)]
        public void CanCheckAfterAllInDuringPreFlopTest2(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
                ("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five, chip3),
            ], true, out var totalChips);
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Check, 0, totalChips, false, false);
        }
        
        [Test]
        [TestCase(Enums.PlayerRoleEnum.SmallBlind, 100, Enums.PlayerRoleEnum.BigBlind, 100)]
        public void TestTest2(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
        {
            var sut = SetupClass.SetupAndDoBlindBet([
                ("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine, chip1),
                ("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine, chip2),
            ], true, out var totalChips);
            //99
            //98
            
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.Call, 0, totalChips, true, false);
            SetupClass.DoAction(sut, Enums.CommandTypeEnum.AllIn, 0, totalChips, false, false);
            Assert.That(sut.GameLogicManager.GetGameState(), Is.EqualTo(Enums.GameStateEnum.Showdown));
        }
    }
}