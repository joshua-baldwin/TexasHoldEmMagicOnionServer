using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Utilities;

namespace TexasHoldEmUnitTests;

public class Tests
{
    public class TestSystem
    {
        public GameLogicManager GameLogicManager { get; } = new GameLogicManager();
        
        public List<CardEntity> GetHand(PlayerEntity player)
        {
            var newList = new List<CardEntity>();
            foreach (var card in GameLogicManager.CommunityCards)
                newList.Add(new CardEntity(card.Suit, card.Rank));
            return player.HoleCards.Concat(new List<CardEntity>(newList)).ToList();
        }
    }
    
    private static PlayerEntity SetupTestHand(string name, Enums.PlayerRoleEnum role, Enums.CardSuitEnum holeCard1Suit, Enums.CardRankEnum holeCard1Rank, Enums.CardSuitEnum holeCard2Suit, Enums.CardRankEnum holeCard2Rank)
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
        var sut = new TestSystem();
        var player = SetupTestHand("1", Enums.PlayerRoleEnum.None, holeCard1Suit, holeCard1Rank, holeCard2Suit, holeCard2Rank);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
        var ranking = HandRankingLogic.GetHandRanking(sut.GetHand(player));
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("1", Enums.PlayerRoleEnum.None, player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank);
        var p2 = SetupTestHand("2", Enums.PlayerRoleEnum.None, player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("1", Enums.PlayerRoleEnum.None, player1HoleCard1Suit, player1HoleCard1Rank, player1HoleCard2Suit, player1HoleCard2Rank);
        var p2 = SetupTestHand("2", Enums.PlayerRoleEnum.None, player2HoleCard1Suit, player2HoleCard1Rank, player2HoleCard2Suit, player2HoleCard2Rank);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out var isGameOver, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }

    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 60, Enums.PlayerRoleEnum.BigBlind, 80, Enums.PlayerRoleEnum.None, 20)]
    public void RaiseTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;

        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;

        sut.GameLogicManager.CreateQueue(players);

        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)

        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)

        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out var isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out isError, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 10, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(sut.GameLogicManager.PreviousPlayer.CurrentBet, Is.EqualTo(15));
        
        var prevBet = sut.GameLogicManager.CurrentPlayer.CurrentBet;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 20, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(sut.GameLogicManager.PreviousPlayer.CurrentBet - prevBet, Is.EqualTo(35));
    }

    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 40, Enums.PlayerRoleEnum.BigBlind, 80)]
    public void AllInWith2Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        
        var players = new List<PlayerEntity> { p1, p2 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = true;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var mainPot = sut.GameLogicManager.Pots[0].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var toReturn = sut.GameLogicManager.Pots[0].PotAmount;
        var sidePot = sut.GameLogicManager.Pots[1].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[2].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out var isError, out _);
        var sbBet = p1.CurrentBet;
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        var p3ChipsBeforeBet = p3.Chips;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];

        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 50)]
    public void AllInWith3PlayersAndLastCannotCallSoTheyGoAllIn(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out var isError, out _);
        var sbBet = p1.CurrentBet;
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        var p3ChipsBeforeBet = p3.Chips;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(!isError);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];

        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 10)]
    public void AllInWith3PlayersAndFirstTwoGoAllInLastFolds(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];

        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
        var winners = sut.GameLogicManager.DoShowdown();
        
        Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
        Assert.That(winners[0].PotToWinner, Is.EqualTo(sidePot));
        
        Assert.That(winners[1].Winner.Id, Is.EqualTo(p2.Id));
        Assert.That(winners[1].PotToWinner, Is.EqualTo(mainPot));
        
        Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToWinner));
        Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[1].PotToWinner));
        Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 70, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80)]
    public void AllInWith3PlayersAllInThenCallThenRaise(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        var prevBet = sut.GameLogicManager.PreviousPlayer.CurrentBet;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(p3.CurrentBet, Is.EqualTo(prevBet));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 40, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 70, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80)]
    public void AllInWith3PlayersAllInThenCallThenCall(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        var prevBet = sut.GameLogicManager.PreviousPlayer.CurrentBet;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(p3.CurrentBet, Is.EqualTo(prevBet));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 7, Enums.PlayerRoleEnum.None, 30)]
    public void AllInWith3PlayersAllInThenCallWithoutReRaise(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        var preFlopTotal = sut.GameLogicManager.Pots[0].PotAmount;
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        var p1PrevBet = sut.GameLogicManager.PreviousPlayer.CurrentBet;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        Assert.That(sut.GameLogicManager.Pots.Count, Is.EqualTo(2));
        Assert.That(sut.GameLogicManager.Pots[1].PotAmount, Is.EqualTo(preFlopTotal + 15));

        var raiseAmount = 5;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, raiseAmount, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        var currentSidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var currentMainPot = sut.GameLogicManager.Pots[1].PotAmount;
        Assert.That(currentSidePot, Is.EqualTo(raiseAmount * 2));
        Assert.That(currentMainPot, Is.EqualTo(preFlopTotal + p1PrevBet * players.Count));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheRiver));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];

        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        var preFlopTotal = sut.GameLogicManager.Pots[0].PotAmount;
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        var p2PrevBet = sut.GameLogicManager.PreviousPlayer.CurrentBet;
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(p3.CurrentBet, Is.EqualTo(p2PrevBet));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        
        var currentSidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var currentMainPot = sut.GameLogicManager.Pots[1].PotAmount;
        Assert.That(currentSidePot, Is.EqualTo(0));
        Assert.That(currentMainPot, Is.EqualTo(preFlopTotal + p2PrevBet * players.Count));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out _, out _);
        Assert.That(isError, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheRiver));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];

        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);

        var initialRaise = 5;
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, initialRaise, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 20, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(sut.GameLogicManager.PreviousPlayer.CurrentBet, Is.EqualTo(35));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 40, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        Assert.That(sut.GameLogicManager.PreviousPlayer.CurrentBet, Is.EqualTo(75 + initialRaise));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        var val = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheRiver));
        var val2 = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val2));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 90, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 80, false, Enums.GameStateEnum.TheTurn)]
    public void AllInWith3PlayersReRaiseAfterAllInThenFold(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, bool isBetError, Enums.GameStateEnum newGameState)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(p3.CurrentBet, Is.EqualTo(chip2 - Constants.MinBet));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 40, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out _, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen);
        
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
        
        var none = players.First(x => x.Name == "none1");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = true;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none1").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var none2ChipsBefore = p4.Chips;
        var sidePotA = sut.GameLogicManager.Pots[0].PotAmount;
        var sidePotB = sut.GameLogicManager.Pots[1].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[2].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 89, Enums.PlayerRoleEnum.None, 42, Enums.PlayerRoleEnum.None, 42)]
    public void AllInWith4PlayersShouldHave2PotsWithSomethingInThem(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen);
        
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
        
        var none = players.First(x => x.Name == "none1");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = true;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none1").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 50, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        Assert.That(sut.GameLogicManager.Pots[0].PotAmount > 0, Is.True);
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 10, Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 38, Enums.PlayerRoleEnum.None, 50)]
    public void AllInWith5Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen);
        var p5 = SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace);
        
        var players = new List<PlayerEntity> { p1, p2, p3, p4, p5 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none1");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = false;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        var none3 = players.First(x => x.Name == "none3");
        none3.IsDealer = true;
        none3.PlayerRole = role5;
        none3.Chips = chip5;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none1").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none3").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheRiver));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var none1ChipsBefore = p3.Chips;
        var none2ChipsBefore = p4.Chips;
        var none3ChipsBefore = p5.Chips;
        var sidePotA = sut.GameLogicManager.Pots[0].PotAmount;
        var sidePotB = sut.GameLogicManager.Pots[1].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[2].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 10, Enums.PlayerRoleEnum.None, 100, Enums.PlayerRoleEnum.None, 37, Enums.PlayerRoleEnum.None, 50)]
    public void AllInWith5PlayersStaggered(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4, Enums.PlayerRoleEnum role5, int chip5)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen);
        var p5 = SetupTestHand("none3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack, Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace);
        
        var players = new List<PlayerEntity> { p1, p2, p3, p4, p5 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none1");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = false;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        var none3 = players.First(x => x.Name == "none3");
        none3.IsDealer = true;
        none3.PlayerRole = role5;
        none3.Chips = chip5;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none1").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Queen)
        ];
        
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none3").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Ace)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 35, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.Pots.Count, Is.EqualTo(3));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        var val = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheRiver));
        var val2 = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val2));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var none1ChipsBefore = p3.Chips;
        var none2ChipsBefore = p4.Chips;
        var none3ChipsBefore = p5.Chips;
        var sidePotA = sut.GameLogicManager.Pots[0].PotAmount;
        var sidePotB = sut.GameLogicManager.Pots[1].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[2].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30)]
    public void TiePotTestWith3Players(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var mainPot = sut.GameLogicManager.Pots[0].PotAmount;
        var winners = sut.GameLogicManager.DoShowdown();
        
        Assert.That(winners[0].TiedWith.Select(x => x.Id), Has.Member(p1.Id));
        Assert.That(winners[0].TiedWith.Select(x => x.Id), Has.Member(p2.Id));
        Assert.That(winners[0].PotToWinner, Is.EqualTo(0));
        Assert.That(winners[0].PotToTiedWith, Is.EqualTo(mainPot / 2));
        
        Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + winners[0].PotToTiedWith));
        Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore + winners[0].PotToTiedWith));
        Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.ThreeOfAKind));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30)]
    public void EveryoneFoldTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role1).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role2).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.PlayerRole == role3).HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out _, out _);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var mainPot = sut.GameLogicManager.Pots[0].PotAmount;
        var winners = sut.GameLogicManager.DoShowdown();
        
        Assert.That(winners[0].Winner.Id, Is.EqualTo(p1.Id));
        Assert.That(winners[0].PotToWinner, Is.EqualTo(mainPot));
        Assert.That(winners[0].HandRanking, Is.EqualTo(Enums.HandRankingType.Nothing));
        Assert.That(p1.Chips, Is.EqualTo(sbChipsBefore + mainPot));
        Assert.That(p2.Chips, Is.EqualTo(bbChipsBefore));
        Assert.That(p3.Chips, Is.EqualTo(p3ChipsBefore));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.GameOver));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 32, Enums.PlayerRoleEnum.None, 32)]
    public void EveryoneFoldAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Seven);
        
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
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = true;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out _, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheFlop));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out isError, out _);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        var val = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 5, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out _, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(sut.GameLogicManager.Pots.All(x => x.PotAmount >= 0), Is.True);
        var val2 = sut.GameLogicManager.PlayerQueue.First().CurrentBet;
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == val2));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var p4ChipsBefore = p4.Chips;
        var sidePot = sut.GameLogicManager.Pots[0].PotAmount;
        var mainPot = sut.GameLogicManager.Pots[1].PotAmount;
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
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 30, Enums.PlayerRoleEnum.None, 30, Enums.PlayerRoleEnum.None, 30)]
    public void FoldDuringPreFlopTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3, Enums.PlayerRoleEnum role4, int chip4)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Seven);
        
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
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = true;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out var isGameOver, out var isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.False);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Fold, 0, out isGameOver, out isError, out _);
        Assert.That(isError, Is.False);
        Assert.That(isGameOver, Is.True);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.GameOver));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        var sbChipsBefore = p1.Chips;
        var bbChipsBefore = p2.Chips;
        var p3ChipsBefore = p3.Chips;
        var p4ChipsBefore = p4.Chips;
        var mainPot = sut.GameLogicManager.Pots[0].PotAmount;
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
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p4 = SetupTestHand("none2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Seven, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Seven);
        
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
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = false;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        var none2 = players.First(x => x.Name == "none2");
        none2.IsDealer = true;
        none2.PlayerRole = role4;
        none2.Chips = chip4;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none2").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.PlayerQueue.All(x => x.CurrentBet == Constants.MinBet));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 25, Enums.PlayerRoleEnum.None, 30)]
    public void GameShouldEndAfterAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 2, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 25, Enums.PlayerRoleEnum.None, 30)]
    public void ThreeWayTieTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight)
        ];
        
        var winners = sut.GameLogicManager.DoShowdown();
        Assert.That(winners.First().TiedWith.Count, Is.EqualTo(3));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 120, Enums.PlayerRoleEnum.BigBlind, 80, Enums.PlayerRoleEnum.None, 60)]
    public void RaiseThenEveryoneAllInTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("small", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        var p2 = SetupTestHand("big", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("none", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        
        var players = new List<PlayerEntity> { p1, p2, p3 };
        sut.GameLogicManager.SetupGame(players, true);
        var sb = players.First(x => x.Name == "small");
        sb.IsDealer = false;
        sb.PlayerRole = role1;
        sb.Chips = chip1;
        
        var bb = players.First(x => x.Name == "big");
        bb.IsDealer = false;
        bb.PlayerRole = role2;
        bb.Chips = chip2;
        
        var none = players.First(x => x.Name == "none");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        sut.GameLogicManager.CreateQueue(players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);

        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "small").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.King),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "big").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Jack),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Nine)
            
        ];
        sut.GameLogicManager.PlayerQueue.First(x => x.Name == "none").HoleCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Two),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five)
        ];
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out var isGameOver, out var isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 60, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.Pots[0].PotAmount, Is.EqualTo(3));
        Assert.That(sut.GameLogicManager.Pots[1].PotAmount, Is.EqualTo(123));
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.Pots[0].PotAmount, Is.EqualTo(5));
        Assert.That(sut.GameLogicManager.Pots[1].PotAmount, Is.EqualTo(181));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.TheTurn));
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 1, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 2, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out isGameOver, out isError, out _);
        Assert.That(!isError);
        Assert.That(!isGameOver);
        Assert.That(sut.GameLogicManager.Pots[0].PotAmount, Is.EqualTo(42));
        Assert.That(sut.GameLogicManager.Pots[1].PotAmount, Is.EqualTo(37));
        Assert.That(sut.GameLogicManager.Pots[2].PotAmount, Is.EqualTo(181));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
        
        sut.GameLogicManager.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Ten),
            new CardEntity(Enums.CardSuitEnum.Club, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight)
        ];
        
        var winners = sut.GameLogicManager.DoShowdown();
        
    }
}