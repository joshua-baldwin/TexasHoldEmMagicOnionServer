using MessagePack;
using THE.Shared.Utilities;
using THE.Shared.Enums;

namespace THE.Entities
{
    [MessagePackObject]
    public class CardEntity
    {
        [Key(0)]
        public Enums.CardSuitEnum Suit { get; private set; }
        
        [Key(1)]
        public Enums.CardRankEnum Rank { get; private set; }
        
        [Key(2)]
        public int Weight { get; set; }
        
        [Key(3)]
        public bool IsFinalHand { get; set; }

        public CardEntity(Enums.CardSuitEnum suit, Enums.CardRankEnum rank)
        {
            Suit = suit;
            Rank = rank;
            Weight = Constants.OriginalCardWeight;
        }
    }
}