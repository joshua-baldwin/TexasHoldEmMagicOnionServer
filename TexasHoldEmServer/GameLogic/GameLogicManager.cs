using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        private Queue<PlayerEntity> playerQueue = new();
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        
        public Enums.CommandTypeEnum PreviousCommandType { get; private set; }
        public int PreviousBetAmount { get; private set; }
        public PlayerEntity CurrentPlayer { get; private set; }
        public int Pot { get; private set; }
        public List<CardEntity> CardPool { get; set; }
        public List<CardEntity> CommunityCards { get; set; } = new List<CardEntity>();
        
        public Enums.GameStateEnum GameState { get; private set; }

        public void SetupGame(List<PlayerEntity> players)
        {
            SetRoles(players);
            //SetCards(players);
            SetChips(players);
            CreateQueue(players);
        }

        public void DoAction(Enums.CommandTypeEnum commandType, int betAmount, out string actionMessage)
        {
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    actionMessage = $"{CurrentPlayer.Name} bet {betAmount}.";
                    PreviousBetAmount = betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    smallBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    actionMessage = $"{CurrentPlayer.Name} bet {betAmount}.";
                    PreviousBetAmount = betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    bigBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.Check:
                    if (PreviousCommandType is Enums.CommandTypeEnum.Call or Enums.CommandTypeEnum.Raise)
                    {
                        actionMessage = "You can't check because a bet has been placed.";
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
                    Pot += PreviousBetAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Raise:
                    if (betAmount < PreviousBetAmount)
                    {
                        actionMessage = "The bet must be greater than the previous bet.";
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} raised {betAmount}.";
                    Pot += betAmount;
                    CurrentPlayer.CurrentBet += betAmount;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }
            PreviousCommandType = commandType;
            playerQueue.Enqueue(CurrentPlayer);
            CurrentPlayer = playerQueue.Dequeue();
            UpdateGameState();
            actionMessage = "";
        }

        private void UpdateGameState()
        {
            switch (GameState)
            {
                case Enums.GameStateEnum.BlindBet:
                {
                    if (smallBlindBetDone && bigBlindBetDone)
                    {
                        GameState = Enums.GameStateEnum.PreFlop;
                        SetCards(playerQueue.ToList());
                    }
                    break;
                }
                case Enums.GameStateEnum.PreFlop:
                {
                    if (!playerQueue.Any(x => !x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheFlop;
                        SetTheFlop();
                    }
                    break;
                }
                case Enums.GameStateEnum.TheFlop:
                {
                    if (!playerQueue.Any(x => !x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheTurn;
                        SetTheTurn();
                    }
                    break;
                }
                case Enums.GameStateEnum.TheTurn:
                {
                    if (!playerQueue.Any(x => !x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheRiver;
                        SetTheRiver();
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            foreach (var player in playerQueue)
                player.HasTakenAction = false;
        }

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
            
            CurrentPlayer = playerQueue.Dequeue();
        }
        
        private void SetTheFlop()
        {
            
        }
        
        private void SetTheTurn()
        {
            
        }

        private void SetTheRiver()
        {
            
        }
    }
}