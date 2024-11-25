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

        #region Check hand
        
        private static bool IsRoyalFlush(CardEntity[] cards)
        {
            var royalFlushCards = new List<Enums.CardRankEnum> { Enums.CardRankEnum.Ace, Enums.CardRankEnum.King, Enums.CardRankEnum.Queen, Enums.CardRankEnum.Jack, Enums.CardRankEnum.Ten };
            foreach (var card in cards)
            {
                if (royalFlushCards.Contains(card.Rank))
                    royalFlushCards.Remove(card.Rank);
            }

            return royalFlushCards.Count == 0;
        }

        private static bool IsStraightFlush(CardEntity[] cards)
        {
            //フラッシュがあった場合そのカードをチェックする
            var flush = cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() == 5);
            return flush != null && IsStraight(flush.Select(x => x).ToArray());
        }

        private static bool IsFourOfAKind(CardEntity[] cards)
        {
            return cards.GroupBy(x => x.Rank).Any(group => group.Count() == 4);
        }

        private static bool IsFullHouse(CardEntity[] cards)
        {
            //ペアか
            if (!IsPair(cards))
                return false;
            //ペア除いて3 of a kindがあるか
            var pair = cards.GroupBy(x => x.Rank).First(x => x.Count() == 2);
            return IsThreeOfAKind(cards.Where(card => card.Rank != pair.Key).ToArray());
        }

        private static bool IsFlush(CardEntity[] cards)
        {
            //7枚の中5枚が同じスーツかをチェック
            return cards.GroupBy(x => x.Suit).Any(x => x.Count() == 5);
        }

        private static bool IsStraight(CardEntity[] cards)
        {
            //7枚の中5枚が連続かをチェック
            var orderedCards = cards.OrderBy(x => x.Rank).ToList();
            var count = 1;
            for (var i = 0; i < orderedCards.Count - 1; i++)
            {
                if (orderedCards[i].Rank + 1 == orderedCards[i + 1].Rank)
                    count++;
            }
            
            return count == 5;
        }

        private static bool IsThreeOfAKind(CardEntity[] cards)
        {
            //同じランクが3枚あるか
            return cards.GroupBy(x => x.Rank).Any(group => group.Count() == 3);
        }

        private static bool IsTwoPair(CardEntity[] cards)
        {
            //ペアが2つあるか
            return cards.GroupBy(x => x.Rank).Count(group => group.Count() == 2) == 2;
        }

        private static bool IsPair(CardEntity[] cards)
        {
            return cards.GroupBy(x => x.Rank).Any(group => group.Count() == 2);
        }

        private static bool IsHighCard(CardEntity[] cards)
        {
            foreach (var card in cards)
            {
                if (card.Rank is
                    Enums.CardRankEnum.Ace or
                    Enums.CardRankEnum.King or
                    Enums.CardRankEnum.Queen or
                    Enums.CardRankEnum.Jack or
                    Enums.CardRankEnum.Ten)
                {
                    return true;
                }
            }

            return false;
        }
        
        #endregion

        #region Compare
        
        private static Guid CompareRoyalFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            //両方royal flush持ってたら引き分け
            return Guid.Empty;
        }
        
        private static Guid CompareStraightFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return CompareStraights(playerHand1, playerHand2);
        }
        
        private static Guid CompareFourOfAKinds((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 4).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 4).ToArray();
            
            //同じランクだった場合、high cardをチェックする
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
        
        private static Guid CompareFullHouses((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            //3 of a kindが同じだった場合ペアをチェックする
            var betterHand = CompareThreeOfAKinds(playerHand1, playerHand2);
            return betterHand == Guid.Empty
                ? ComparePairs(playerHand1, playerHand2)
                : betterHand;
        }
        
        private static Guid CompareFlushes((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return CompareHighCards(playerHand1, playerHand2);
        }
        
        private static Guid CompareStraights((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.OrderByDescending(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.OrderByDescending(x => x.Rank).ToArray();

            //一番高いランクが同じだった場合引き分け
            if (hand1[0].Rank == hand2[0].Rank)
                return Guid.Empty;
            
            return hand1[0].Rank > hand2[0].Rank ? playerHand1.Item1 : playerHand2.Item1;
        }
        
        private static Guid CompareThreeOfAKinds((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 3).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).Where(x => x.Count() == 3).ToArray();

            //同じランクだった場合、high cardをチェックする
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
        
        private static Guid CompareTwoPairs((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            return ComparePairs(playerHand1, playerHand2);
        }
        
        private static Guid ComparePairs((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.GroupBy(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.GroupBy(x => x.Rank).ToArray();

            //高いランクのペアが勝ち
            //全部同じだった場合引き分け
            var betterHand = Guid.Empty;
            for (var i = 0; i < hand1.Length; i++)
            {
                if (hand1[i].Key == hand2[i].Key)
                    continue;
                
                betterHand = hand1[i].Key > hand2[i].Key ? playerHand1.Item1 : playerHand2.Item1;
            }
            
            return betterHand;
        }
        
        private static Guid CompareHighCards((Guid, CardEntity[]) playerHand1, (Guid, CardEntity[]) playerHand2)
        {
            var hand1 = playerHand1.Item2.OrderByDescending(x => x.Rank).ToArray();
            var hand2 = playerHand2.Item2.OrderByDescending(x => x.Rank).ToArray();

            //一番高いランクが勝ち
            //全部同じだった場合引き分け
            var betterHand = Guid.Empty;
            for (int i = 0; i < hand1.Length; i++)
            {
                if (hand1[i].Rank == hand2[i].Rank)
                    continue;

                betterHand = hand1[i].Rank > hand2[i].Rank
                    ? playerHand1.Item1
                    : playerHand2.Item1;
            }
            
            return betterHand;
        }
        
        #endregion
    }
}