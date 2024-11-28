using TexasHoldEmServer.GameLogic;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmUnitTests;

public class Tests
{
    public class TestSystem
    {
        public GameLogicManager GameLogicManager { get; set; } = new GameLogicManager();
        public List<PlayerEntity> Players { get; set; } = new();
        public List<CardEntity> CommunityCards { get; set; } = new();

        public void Initialize()
        {
            GameLogicManager.SetupGame(Players);
            GameLogicManager.CreateQueue(Players);
        }
        
        public List<CardEntity> GetHand(PlayerEntity player)
        {
            var newList = new List<CardEntity>();
            foreach (var card in CommunityCards)
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
        sut.Players.Add(player);
        sut.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
        var ranking = HandRankingLogic.GetHandRanking(sut.GetHand(player).ToArray());
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
        sut.Players.Add(p1);
        sut.Players.Add(p2);
        sut.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
        var p1Hand = sut.GetHand(p1).ToArray();
        var p2Hand = sut.GetHand(p2).ToArray();
        var ranking1 = HandRankingLogic.GetHandRanking(p1Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(p2Hand);
        var winner = HandRankingLogic.CompareHands((p1.Id, p1Hand.Where(x => x.IsFinalHand).ToArray()), (p2.Id, p2Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
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
        sut.Players.Add(p1);
        sut.Players.Add(p2);
        sut.CommunityCards =
        [
            new CardEntity(communityCard1Suit, communityCard1Rank),
            new CardEntity(communityCard2Suit, communityCard2Rank),
            new CardEntity(communityCard3Suit, communityCard3Rank),
            new CardEntity(communityCard4Suit, communityCard4Rank),
            new CardEntity(communityCard5Suit, communityCard5Rank)
        ];
        var p1Hand = sut.GetHand(p1).ToArray();
        var p2Hand = sut.GetHand(p2).ToArray();
        var ranking1 = HandRankingLogic.GetHandRanking(p1Hand);
        var ranking2 = HandRankingLogic.GetHandRanking(p2Hand);
        var winner = HandRankingLogic.CompareHands((p1.Id, p1Hand.Where(x => x.IsFinalHand).ToArray()), (p2.Id, p2Hand.Where(x => x.IsFinalHand).ToArray()), handRankingType);
        
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
        var p1 = SetupTestHand("1", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p2 = SetupTestHand("2", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        sut.Players.Add(p1);
        sut.Players.Add(p2);
        sut.Players.Add(p3);
        sut.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        sut.Initialize();
        sut.Players.First(x => x.PlayerRole == role1).Chips = chip1;
        sut.Players.First(x => x.PlayerRole == role2).Chips = chip2;
        sut.Players.First(x => x.PlayerRole == role3).Chips = chip3;
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
    
    [Test]
    [TestCase(Enums.PlayerRoleEnum.SmallBlind, 50, Enums.PlayerRoleEnum.BigBlind, 20, Enums.PlayerRoleEnum.None, 10)]
    public void ShowdownTest(Enums.PlayerRoleEnum role1, int chip1, Enums.PlayerRoleEnum role2, int chip2, Enums.PlayerRoleEnum role3, int chip3)
    {
        var sut = new TestSystem();
        var p1 = SetupTestHand("1", Enums.PlayerRoleEnum.BigBlind, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Eight, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Five);
        var p2 = SetupTestHand("2", Enums.PlayerRoleEnum.SmallBlind, Enums.CardSuitEnum.Club, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Nine);
        var p3 = SetupTestHand("3", Enums.PlayerRoleEnum.None, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Ten, Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine);
        sut.Players.Add(p1);
        sut.Players.Add(p2);
        sut.Players.Add(p3);
        sut.CommunityCards =
        [
            new CardEntity(Enums.CardSuitEnum.Heart, Enums.CardRankEnum.Five),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Eight),
            new CardEntity(Enums.CardSuitEnum.Spade, Enums.CardRankEnum.Nine),
            new CardEntity(Enums.CardSuitEnum.Diamond, Enums.CardRankEnum.Nine)
        ];
        
        //sut.Initialize();
        sut.GameLogicManager.SetupGame(sut.Players);
        var bb = sut.Players.First(x => x.Name == "1");
        bb.PlayerRole = role1;
        bb.Chips = chip1;
        
        var sb = sut.Players.First(x => x.Name == "2");
        sb.PlayerRole = role2;
        sb.Chips = chip2;
        var none = sut.Players.First(x => x.Name == "3");
        none.IsDealer = true;
        none.PlayerRole = role3;
        none.Chips = chip3;
        
        
        sut.GameLogicManager.CreateQueue(sut.Players);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.SmallBlindBet, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.BigBlindBet, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Call, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Check, 0, out _, out _, out _);
        
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.Raise, 25, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);
        sut.GameLogicManager.DoAction(Enums.CommandTypeEnum.AllIn, 0, out _, out _, out _);

        var winners = sut.GameLogicManager.DoShowdown();
        
        Assert.That(winners.First().Item1.First(), Is.EqualTo(p2.Id));
        Assert.That(winners.First().Item2, Is.EqualTo(Enums.HandRankingType.FullHouse));
        Assert.That(sut.GameLogicManager.GameState, Is.EqualTo(Enums.GameStateEnum.Showdown));
    }
}