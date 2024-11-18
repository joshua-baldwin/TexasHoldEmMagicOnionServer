using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        public static int MaxPlayers = 10;
        private Queue<PlayerEntity> playerQueue = new();
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private readonly Dictionary<Guid, CardEntity[]> playerHands = new();
        
        public Enums.CommandTypeEnum PreviousCommandType { get; private set; }
        public int PreviousBetAmount { get; private set; }
        public PlayerEntity CurrentPlayer { get; private set; }
        public int Pot { get; private set; }
        public int TotalBetForTurn { get; private set; }
        public List<CardEntity> CardPool { get; set; }
        public CardEntity[] CommunityCards { get; } = new CardEntity[5];
        
        public Enums.GameStateEnum GameState { get; private set; }

        public void SetupGame(List<PlayerEntity> players)
        {
            SetRoles(players);
            SetChips(players);
            CreateQueue(players);
        }

        public void DoAction(Enums.CommandTypeEnum commandType, int betAmount, out bool isError, out string actionMessage)
        {
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    if (!CanPlaceBet(betAmount, out var message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {betAmount}.";
                    TotalBetForTurn += betAmount;
                    PreviousBetAmount = betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    smallBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    if (!CanPlaceBet(betAmount, out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    if (betAmount > CurrentPlayer.Chips.Sum(x => (int)x.ChipType))
                    {
                        actionMessage = "Not enough chips.";
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {betAmount}.";
                    TotalBetForTurn += betAmount;
                    PreviousBetAmount = betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    bigBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.Check:
                    if (PreviousCommandType is Enums.CommandTypeEnum.Call or Enums.CommandTypeEnum.Raise)
                    {
                        actionMessage = "You can't check because a bet has been placed.";
                        isError = true;
                        return;
                    }
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Fold:
                    actionMessage = $"{CurrentPlayer.Name} folded.";
                    CurrentPlayer.HasFolded = true;
                    break;
                case Enums.CommandTypeEnum.Call:
                    actionMessage = $"{CurrentPlayer.Name} called.";
                    TotalBetForTurn += PreviousBetAmount;
                    CurrentPlayer.CurrentBet += PreviousBetAmount;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Raise:
                    if (!CanPlaceBet(betAmount, out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} raised {betAmount}.";
                    TotalBetForTurn += betAmount;
                    PreviousBetAmount = betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }
            PreviousCommandType = commandType;
            playerQueue.Dequeue();
            playerQueue.Enqueue(CurrentPlayer);
            CurrentPlayer = playerQueue.Peek();
            UpdateGameState();
            actionMessage = "";
            isError = false;
        }

        private void UpdateGameState()
        {
            var gameStateChanged = false;
            switch (GameState)
            {
                case Enums.GameStateEnum.BlindBet:
                    if (smallBlindBetDone && bigBlindBetDone)
                    {
                        GameState = Enums.GameStateEnum.PreFlop;
                        SetCards(playerQueue.ToList());
                        gameStateChanged = true;
                    }
                    break;
                case Enums.GameStateEnum.PreFlop:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheFlop;
                        SetTheFlop();
                        gameStateChanged = true;
                        UpdatePot();
                    }
                    break;
                case Enums.GameStateEnum.TheFlop:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheTurn;
                        SetTheTurn();
                        gameStateChanged = true;
                        UpdatePot();
                        ResetBets();
                    }
                    break;
                case Enums.GameStateEnum.TheTurn:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheRiver;
                        SetTheRiver();
                        gameStateChanged = true;
                        UpdatePot();
                        ResetBets();
                    }
                    break;
                case Enums.GameStateEnum.TheRiver:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.Showdown;
                        gameStateChanged = true;
                        UpdatePot();
                        ResetBets();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!gameStateChanged)
                return;
            
            CurrentPlayer.HasTakenAction = false;
            foreach (var player in playerQueue)
                player.HasTakenAction = false;
        }

        #region Setup
        
        private void SetRoles(List<PlayerEntity> players)
        {
            var first = players.GetRandomElement();
            first.IsDealer = true;
            CardPool = CreateDeck();

            var index = players.IndexOf(first);
            index = index + 1 >= players.Count ? 0 : index + 1;
            players[index].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
            index = index + 1 >= players.Count ? 0 : index + 1;
            players[index].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
        }

        private void SetCards(List<PlayerEntity> players)
        {
            var dealer = players.First(x => x.IsDealer);
            var deck = CardPool;
            var shuffled = deck.Shuffle();
            //give to dealer last
            var dealerIndex = players.IndexOf(dealer);
            var startIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
            var dealt = 0;
            while (dealt < players.Count)
            {
                var card1 = shuffled.GetRandomElement();
                shuffled.Remove(card1);
                var card2 = shuffled.GetRandomElement();
                shuffled.Remove(card2);
                players[startIndex].HoleCards[0] = card1;
                players[startIndex].HoleCards[1] = card2;
                dealt++;
                if (startIndex + 1 >= players.Count)
                    startIndex = 0;
                else
                    startIndex++;
            }

            CardPool = shuffled;
        }
        
        private void SetChips(List<PlayerEntity> players)
        {
            //start with 50 chips
            //25 + 10 + 5 + 5 + 1 + 1 + 1 + 1 + 1
            foreach (var player in players)
            {
                var chips = new List<ChipEntity>();
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Green));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Blue));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Red));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.Red));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                chips.Add(new ChipEntity(Enums.ChipTypeEnum.White));
                player.Chips = chips;
            }
        }
        
        private List<CardEntity> CreateDeck()
        {
            var suits = Enum.GetValues<Enums.CardSuitEnum>();
            var ranks = Enum.GetValues<Enums.CardRankEnum>();
            var deck = new List<CardEntity>();
            foreach (var suit in suits)
            {
                deck.AddRange(ranks.Select(rank => new CardEntity(suit, rank)));
            }

            return deck;
        }
        
        private void CreateQueue(List<PlayerEntity> players)
        {
            var index = players.FindIndex(player => player.PlayerRole == Enums.PlayerRoleEnum.SmallBlind);
            while (playerQueue.Count < players.Count)
            {
                playerQueue.Enqueue(players[index]);
                if (index + 1 >= players.Count)
                    index = 0;
                else
                    index++;
            }
            
            CurrentPlayer = playerQueue.Peek();
        }
        
        #endregion
        
        #region Setting community cards
        
        private void SetTheFlop()
        {
            var card1 = CardPool.GetRandomElement();
            CardPool.Remove(card1);
            var card2 = CardPool.GetRandomElement();
            CardPool.Remove(card2);
            var card3 = CardPool.GetRandomElement();
            CardPool.Remove(card3);
            
            CommunityCards[0] = card1;
            CommunityCards[1] = card2;
            CommunityCards[2] = card3;
        }
        
        private void SetTheTurn()
        {
            var card4 = CardPool.GetRandomElement();
            CardPool.Remove(card4);
            CommunityCards[3] = card4;
        }

        private void SetTheRiver()
        {
            var card5 = CardPool.GetRandomElement();
            CardPool.Remove(card5);
            CommunityCards[4] = card5;
        }
        
        #endregion

        public void SetPlayerHand(Guid playerId, CardEntity[] showdownCards)
        {
            playerHands.Add(playerId, showdownCards);
        }
        
        public Guid GetWinner(CardEntity[] showdownCards)
        {
            var winnerId = Guid.Empty;
            foreach (var hand in playerHands)
            {
                
            }
            return winnerId;
        }
        
        private bool CanPlaceBet(int betAmount, out string message)
        {
            if (betAmount <= 0)
            {
                message = "The bet must be greater than 0.";
                return false;
            }
            
            if (PreviousBetAmount != 0 && betAmount <= PreviousBetAmount)
            {
                message = "The bet must be greater than the previous bet.";
                return false;
            }

            if (betAmount > CurrentPlayer.Chips.Sum(x => (int)x.ChipType))
            {
                message = "Not enough chips.";
                return false;
            }

            message = "";
            return true;
        }
        
        private void UpdatePot()
        {
            Pot = TotalBetForTurn;
            TotalBetForTurn = 0;
        }

        private void ResetBets()
        {
            foreach (var player in playerQueue)
                player.CurrentBet = 0;
        }
    }
}