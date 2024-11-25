using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten,
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.Queen,
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack,
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight,
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.King,
        Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight,
        Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ace)]
    public void HandIsRoyalFlush(
        Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank,
        Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank,
        Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank,
        Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank,
        Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank,
        Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank,
        Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank)
    {
        var holeCards = new CardEntity[]
        {
            new CardEntity(holeCard1Suit, holeCard1Rank),
            new CardEntity(holeCard2Suit, holeCard2Rank),
        };
        
        var communityCards = new List<CardEntity>
        {
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank),
        };
        
        var ranking = HandRankingLogic.GetHandRanking(communityCards.Concat(holeCards).ToArray());
        
        Assert.That(ranking, Is.EqualTo(Enums.HandRankingType.RoyalFlush));
    }
}