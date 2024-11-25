using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests;

public class Tests
{
    public class TestHand
    {
        public List<CardEntity> HoleCards { get; set; } = new();
        public List<CardEntity> CommunityCards { get; set; } = new();

        public CardEntity[] Hand => CommunityCards.Concat(HoleCards).ToArray();
    }
    
    public static TestHand SetupTestHand(Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank, Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank, Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank, Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank, Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank, Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank, Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank)
    {
        var testHand = new TestHand
        {
            HoleCards =
            [
                new CardEntity(holeCard1Suit, holeCard1Rank),
                new CardEntity(holeCard2Suit, holeCard2Rank)
            ],
            CommunityCards =
            [
                new CardEntity(communityCard1Suit, communityCard1Rank),
                new CardEntity(communityCard2Suit, communityCard2Rank),
                new CardEntity(communityCard3Suit, communityCard3Rank),
                new CardEntity(communityCard4Suit, communityCard4Rank),
                new CardEntity(communityCard5Suit, communityCard5Rank)
            ]
        };
        return testHand;
    }

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
    public void HandIs(
        Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank,
        Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank,
        Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank,
        Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank,
        Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank,
        Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank,
        Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank,
        Enums.HandRankingType handRankingType)
    {
        var sut = SetupTestHand(holeCard1Suit, holeCard1Rank, holeCard2Suit, holeCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var ranking = HandRankingLogic.GetHandRanking(sut.CommunityCards.Concat(sut.HoleCards).ToArray());
        
        Assert.That(ranking, Is.EqualTo(handRankingType));
    }
}