using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests;

public class Tests
{
    public class TestPlayer
    {
        public Guid PlayerId { get; set; } = Guid.NewGuid();
        public List<CardEntity> HoleCards { get; set; } = new();
        public List<CardEntity> CommunityCards { get; set; } = new();

        public CardEntity[] Hand => CommunityCards.Concat(HoleCards).ToArray();
    }
    
    public static TestPlayer SetupTestHand(Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank, Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank, Enums.CardSuitEnum communityCard1Suit, Enums.CardRankEnum communityCard1Rank, Enums.CardSuitEnum communityCard2Suit, Enums.CardRankEnum communityCard2Rank, Enums.CardSuitEnum communityCard3Suit, Enums.CardRankEnum communityCard3Rank, Enums.CardSuitEnum communityCard4Suit, Enums.CardRankEnum communityCard4Rank, Enums.CardSuitEnum communityCard5Suit, Enums.CardRankEnum communityCard5Rank)
    {
        var testHand = new TestPlayer
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
        var sut = SetupTestHand(holeCard1Suit, holeCard1Rank, holeCard2Suit, holeCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var ranking = HandRankingLogic.GetHandRanking(sut.Hand);
        
        Assert.That(ranking, Is.EqualTo(handRankingType));
    }

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
        var sut = SetupTestHand(player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var sut2 = SetupTestHand(player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        
        var ranking1 = HandRankingLogic.GetHandRanking(sut.Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(sut2.Hand);
        var winner = HandRankingLogic.CompareHands((sut.PlayerId, sut.Hand.Where(x => x.IsFinalHand).ToArray()), (sut2.PlayerId, sut2.Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
        Assert.That(ranking1, Is.EqualTo(handRankingType));
        Assert.That(ranking2, Is.EqualTo(handRankingType));
        Assert.That(sut.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.PlayerId, Is.EqualTo(winner));
    }
    
    [Test]
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
    public void CompareFourOfAKindTest(Enums.CardSuitEnum player1HoleCard1Suit, Enums.CardRankEnum player1HoleCard1Rank,
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
        var sut = SetupTestHand(player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var sut2 = SetupTestHand(player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        
        var ranking1 = HandRankingLogic.GetHandRanking(sut.Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(sut2.Hand);
        var winner = HandRankingLogic.CompareHands((sut.PlayerId, sut.Hand.Where(x => x.IsFinalHand).ToArray()), (sut2.PlayerId, sut2.Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
        Assert.That(ranking1, Is.EqualTo(handRankingType));
        Assert.That(ranking2, Is.EqualTo(handRankingType));
        Assert.That(sut.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.PlayerId, Is.EqualTo(winner));
    }
    
    [Test]
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
    public void CompareThreeOfAKindTest(Enums.CardSuitEnum player1HoleCard1Suit, Enums.CardRankEnum player1HoleCard1Rank,
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
        var sut = SetupTestHand(player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var sut2 = SetupTestHand(player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        
        var ranking1 = HandRankingLogic.GetHandRanking(sut.Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(sut2.Hand);
        var winner = HandRankingLogic.CompareHands((sut.PlayerId, sut.Hand.Where(x => x.IsFinalHand).ToArray()), (sut2.PlayerId, sut2.Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
        Assert.That(ranking1, Is.EqualTo(handRankingType));
        Assert.That(ranking2, Is.EqualTo(handRankingType));
        Assert.That(sut.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.PlayerId, Is.EqualTo(winner));
    }
    
    [Test]
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
    public void CompareHighCardsTest(Enums.CardSuitEnum player1HoleCard1Suit, Enums.CardRankEnum player1HoleCard1Rank,
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
        var sut = SetupTestHand(player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        var sut2 = SetupTestHand(player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank, communityCard1Suit, communityCard1Rank, communityCard2Suit, communityCard2Rank, communityCard3Suit, communityCard3Rank, communityCard4Suit, communityCard4Rank, communityCard5Suit, communityCard5Rank);
        
        var ranking1 = HandRankingLogic.GetHandRanking(sut.Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(sut2.Hand);
        var winner = HandRankingLogic.CompareHands((sut.PlayerId, sut.Hand.Where(x => x.IsFinalHand).ToArray()), (sut2.PlayerId, sut2.Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
        Assert.That(ranking1, Is.EqualTo(handRankingType));
        Assert.That(ranking2, Is.EqualTo(handRankingType));
        Assert.That(sut.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.Hand.Count(x => x.IsFinalHand), Is.EqualTo(5));
        Assert.That(sut2.PlayerId, Is.EqualTo(winner));
    }
}