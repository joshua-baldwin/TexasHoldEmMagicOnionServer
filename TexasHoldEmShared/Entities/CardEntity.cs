using MessagePack;
using TexasHoldEmShared.Enums;

namespace THE.MagicOnion.Shared.Entities
{
    [MessagePackObject]
    public class CardEntity
    {
        [Key(0)]
        public Enums.CardSuitEnum Suit { get; private set; }
        
        [Key(1)]
        public Enums.CardRankEnum Rank { get; private set; }

        public CardEntity(Enums.CardSuitEnum suit, Enums.CardRankEnum rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}