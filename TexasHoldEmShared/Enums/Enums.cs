using System.ComponentModel;

namespace THE.Shared.Enums
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
            None,
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
            [Description("ロイヤルフラッシュ")]
            RoyalFlush = 1,
            [Description("ストレートフラッシュ")]
            StraightFlush,
            [Description("フォーカード")]
            FourOfAKind,
            [Description("フルハウス")]
            FullHouse,
            [Description("フラッシュ")]
            Flush,
            [Description("ストレート")]
            Straight,
            [Description("スリーカード")]
            ThreeOfAKind,
            [Description("ツーペア")]
            TwoPair,
            [Description("ペア")]
            Pair,
            [Description("ハイカード")]
            HighCard,
            [Description("無し")]
            Nothing
        }

        public enum StartResponseTypeEnum
        {
            Success,
            AllPlayersNotReady,
            NotEnoughChips,
            NotEnoughPlayers,
            GroupDoesNotExist,
            AlreadyPlayedMaxRounds,
            Failed,
            InternalServerError,
        }

        public enum JoinRoomResponseTypeEnum
        {
            Success,
            AllRoomsFull,
            Failed,
            InternalServerError,
        }
        
        public enum BuyJokerResponseTypeEnum
        {
            Success,
            NotEnoughChips,
            GroupDoesNotExist,
            Failed,
            InternalServerError,
        }
        
        public enum UseJokerResponseTypeEnum
        {
            Success,
            NotEnoughChips,
            GroupDoesNotExist,
            Failed,
            PlayerHasInvalidCardData,
            InternalServerError,
        }

        public enum DoActionResponseTypeEnum
        {
            Success,
            GroupDoesNotExist,
            Failed,
            PlayerHasInvalidCardData,
            InternalServerError,
        }

        public enum JokerTypeEnum
        {
            Hand,
            Action,
            Info,
            Board
        }

        public enum HandInfluenceTypeEnum
        {
            None,
            DrawThenDiscard,
            DiscardThenDraw,
            DrawCard,
        }
        
        public enum ActionInfluenceTypeEnum
        {
            None,
            Force,
            Prevent,
            ChangePosition,
            ChangeStack,
            IncreaseBettingRounds,
            PreventJoker,
        }

        public enum InfoInfluenceTypeEnum
        {
            None,
            CheckHand,
        }

        public enum BoardInfluenceTypeEnum
        {
            None,
            IncreaseCardWeight,
            DecreaseCardWeight,
            PreventCommunityCard,
        }

        public enum TargetTypeEnum
        {
            None,
            Self,
            SinglePlayer,
            MultiPlayers,
            All
        }
    }
}