using System.ComponentModel;

namespace TexasHoldEmShared.Enums
{
    public static class Enums
    {
        public enum PlayerRoleEnum
        {
            None,
            SmallBlind,
            BigBlind
        }
        
        public enum CardSuitEnum
        {
            Heart,
            Spade,
            Diamond,
            Club,
            None
        }
        
        public enum CardRankEnum
        {
            [Description("JK")]
            Joker = 0,
            [Description("2")]
            Two = 2,
            [Description("3")]
            Three,
            [Description("4")]
            Four,
            [Description("5")]
            Five,
            [Description("6")]
            Six,
            [Description("7")]
            Seven,
            [Description("8")]
            Eight,
            [Description("9")]
            Nine,
            [Description("10")]
            Ten,
            [Description("J")]
            Jack,
            [Description("Q")]
            Queen,
            [Description("K")]
            King,
            [Description("A")]
            Ace
        }
        
        public enum CommandTypeEnum
        {
            SmallBlindBet,
            BigBlindBet,
            Check,
            Fold,
            Call,
            Raise,
            AllIn
        }
        
        public enum GameStateEnum
        {
            BlindBet,
            PreFlop,
            TheFlop,
            TheTurn,
            TheRiver,
            Showdown,
            GameOver
        }
        
        public enum HandRankingType
        {
            RoyalFlush = 1,
            StraightFlush,
            FourOfAKind,
            FullHouse,
            Flush,
            Straight,
            ThreeOfAKind,
            TwoPair,
            Pair,
            HighCard,
            Nothing
        }
    }
}