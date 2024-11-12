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
            Club
        }
        
        public enum CardRankEnum
        {
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
            Check,
            Bet,
            Fold,
            Call,
            Raise
        }
        
        public enum ChipTypeEnum
        {
            White = 1,
            Red = 5,
            Blue = 10,
            Green = 25,
            Black = 100
        }
    }
}