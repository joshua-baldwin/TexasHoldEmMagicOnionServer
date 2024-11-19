using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.GameLogic
{
    public static class HandRankingLogic
    {
        public static Enums.HandRankingType GetHandRanking(CardEntity[] hand)
        {
            if (IsRoyalFlush(hand))
                return Enums.HandRankingType.RoyalFlush;
            if (IsStraightFlush(hand))
                return Enums.HandRankingType.StraightFlush;
            if (IsFourOfAKind(hand))
                return Enums.HandRankingType.FourOfAKind;
            if (IsFullHouse(hand))
                return Enums.HandRankingType.FullHouse;
            if (IsFlush(hand))
                return Enums.HandRankingType.Flush;
            if (IsStraight(hand))
                return Enums.HandRankingType.Straight;
            if (IsThreeOfAKind(hand))
                return Enums.HandRankingType.ThreeOfAKind;
            if (IsTwoPair(hand))
                return Enums.HandRankingType.TwoPair;
            if (IsPair(hand))
                return Enums.HandRankingType.Pair;
            if (IsHighCard(hand))
                return Enums.HandRankingType.HighCard;

            return Enums.HandRankingType.Nothing;
        }

        public static Guid CompareHands((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2, Enums.HandRankingType handRanking)
        {
            switch (handRanking)
            {
                case Enums.HandRankingType.RoyalFlush:
                    return CompareRoyalFlushes(playerHand1, playerHand2);
                case Enums.HandRankingType.StraightFlush:
                    return CompareStraightFlushes(playerHand1, playerHand2);
                case Enums.HandRankingType.FourOfAKind:
                    return CompareFourOfAKinds(playerHand1, playerHand2);
                case Enums.HandRankingType.FullHouse:
                    return CompareFullHouses(playerHand1, playerHand2);
                case Enums.HandRankingType.Flush:
                    return CompareFlushes(playerHand1, playerHand2);
                case Enums.HandRankingType.Straight:
                    return CompareStraights(playerHand1, playerHand2);
                case Enums.HandRankingType.ThreeOfAKind:
                    return CompareThreeOfAKinds(playerHand1, playerHand2);
                case Enums.HandRankingType.TwoPair:
                    return CompareTwoPairs(playerHand1, playerHand2);
                case Enums.HandRankingType.Pair:
                    return ComparePairs(playerHand1, playerHand2);
                case Enums.HandRankingType.HighCard:
                    return CompareHighCards(playerHand1, playerHand2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(handRanking), handRanking, null);
            }
        }
        
        private static bool IsRoyalFlush(CardEntity[] cards)
        {
            var possible = new List<Enums.CardRankEnum> { Enums.CardRankEnum.Ace, Enums.CardRankEnum.King, Enums.CardRankEnum.Queen, Enums.CardRankEnum.Jack, Enums.CardRankEnum.Ten };
            var valid = true;
            foreach (var card in cards)
            {
                if (possible.Contains(card.Rank))
                {
                    possible.Remove(card.Rank);
                    continue;
                }

                valid = false;
                break;
            }

            return valid;
        }

        private static Guid CompareRoyalFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            //ranks are always the same so its a tie
            return Guid.Empty;
        }

        private static bool IsStraightFlush(CardEntity[] cards)
        {
            return IsStraight(cards) && IsFlush(cards);
        }
        
        private static Guid CompareStraightFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return CompareStraights(playerHand1, playerHand2);
        }

        private static bool IsFourOfAKind(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).ToList();
            return ranks.GroupBy(x => x).Any(group => group.Count() == 4);
        }
        
        private static Guid CompareFourOfAKinds((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 4).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 4).ToArray();

            Guid betterHand;
            if (hand1[0].Key == hand2[0].Key)
            {
                var arg1 = (playerHand1.Item1, playerHand1.Item2.Where(x => x.Rank != hand1[0].Key).ToArray());
                var arg2 = (playerHand2.Item1, playerHand2.Item2.Where(x => x.Rank != hand2[0].Key).ToArray());
                betterHand = CompareHighCards(arg1, arg2);
            }
            else
            {
                betterHand = hand1.Rank > hand2.Rank ? playerHand1.Item1 : playerHand2.Item1;
            }
            
            return betterHand;   
        }

        private static bool IsFullHouse(CardEntity[] cards)
        {
            if (!IsPair(cards))
                return false;
            
            var pair = cards.Select(x => x.Rank).GroupBy(x => x).First(x => x.Count() == 2);
            return IsThreeOfAKind(cards.Where(card => card.Rank != pair.Key).ToArray());
        }
        
        private static Guid CompareFullHouses((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var betterHand = CompareThreeOfAKinds(playerHand1, playerHand2);
            return betterHand == Guid.Empty
                ? ComparePairs(playerHand1, playerHand2)
                : betterHand;
        }

        private static bool IsFlush(CardEntity[] cards)
        {
            var suits = cards.Select(c => c.Suit).Distinct().ToList();
            return suits.Count == 1;
        }
        
        private static Guid CompareFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return CompareHighCards(playerHand1, playerHand2);
        }

        private static bool IsStraight(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).Order().ToList();
            return ranks.Zip(ranks.Skip(1), (a, b) => (a + 1) == b).All(x => x);
        }
        
        private static Guid CompareStraights((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.OrderByDescending(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.OrderByDescending(x => x.Rank).ToArray();

            if (hand1[0].Rank == hand2[0].Rank)
                return Guid.Empty;
            
            return hand1[0].Rank > hand2[0].Rank ? playerHand1.Item1 : playerHand2.Item1;
        }

        private static bool IsThreeOfAKind(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).ToList();
            return ranks.GroupBy(x => x).Any(group => group.Count() == 3);
        }
        
        private static Guid CompareThreeOfAKinds((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 3).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 3).ToArray();

            Guid betterHand;
            if (hand1[0].Key == hand2[0].Key)
            {
                var arg1 = (playerHand1.Item1, playerHand1.Item2.Where(x => x.Rank != hand1[0].Key).ToArray());
                var arg2 = (playerHand2.Item1, playerHand2.Item2.Where(x => x.Rank != hand2[0].Key).ToArray());
                betterHand = CompareHighCards(arg1, arg2);
            }
            else
            {
                betterHand = hand1.Rank > hand2.Rank ? playerHand1.Item1 : playerHand2.Item1;
            }
            
            return betterHand;
        }

        private static bool IsTwoPair(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).ToList();
            var groups = ranks.GroupBy(x => x);
            return groups.Count(group => group.Count() == 2) == 2;
        }
        
        private static Guid CompareTwoPairs((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return ComparePairs(playerHand1, playerHand2);
        }

        private static bool IsPair(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).ToList();
            return ranks.GroupBy(x => x).Any(group => group.Count() == 2);
        }
        
        private static Guid ComparePairs((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).ToArray();

            var betterHand = Guid.Empty;
            for (var i = 0; i < hand1.Length; i++)
            {
                if (hand1[i].Key == hand2[i].Key)
                    continue;
                
                betterHand = hand1[i].Key > hand2[i].Key ? playerHand1.Item1 : playerHand2.Item1;
            }
            
            //if empty its a tie
            return betterHand;
        }

        private static bool IsHighCard(CardEntity[] cards)
        {
            var ranks = cards.Select(card => card.Rank).ToList();
            return ranks.Contains(Enums.CardRankEnum.Ace) ||
                   ranks.Contains(Enums.CardRankEnum.King) ||
                   ranks.Contains(Enums.CardRankEnum.Queen) ||
                   ranks.Contains(Enums.CardRankEnum.Jack) ||
                   ranks.Contains(Enums.CardRankEnum.Ten);
        }
        
        private static Guid CompareHighCards((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.OrderByDescending(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.OrderByDescending(x => x.Rank).ToArray();

            var betterHand = Guid.Empty;
            for (int i = 0; i < hand1.Length; i++)
            {
                if (hand1[i].Rank == hand2[i].Rank)
                    continue;

                betterHand = hand1[i].Rank > hand2[i].Rank
                    ? playerHand1.Item1
                    : playerHand2.Item1;
            }
            //if empty its a tie
            return betterHand;
        }
    }
}