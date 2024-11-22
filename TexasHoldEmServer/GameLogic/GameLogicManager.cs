using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        public const int MaxPlayers = 10;
        public const int MaxHoleCards = 2;
        public const int StartingChips = 50;
        private readonly Queue<PlayerEntity> playerQueue = new();
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private Enums.CommandTypeEnum previousCommandType;
        private int previousBet;
        private int totalChipsForTurn;
        private List<CardEntity> cardPool;
        private readonly Dictionary<Guid, CardEntity[]> playerHands = new();
        private bool isTie;
        
        public PlayerEntity CurrentPlayer { get; private set; }
        public int Pot { get; private set; }
        public CardEntity[] CommunityCards { get; } = new CardEntity[5];
        public Enums.GameStateEnum GameState { get; private set; }

        public void Reset()
        {
            playerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousCommandType = 0;
            previousBet = 0;
            totalChipsForTurn = 0;
            cardPool.Clear();
            playerHands.Clear();
            isTie = false;
            CurrentPlayer = null;
            Pot = 0;
            for (var i = 0; i < CommunityCards.Length; i++)
                CommunityCards[i] = null;
            GameState = Enums.GameStateEnum.BlindBet;
        }
        public void SetupGame(ref List<PlayerEntity> players)
        {
            cardPool = CreateDeck();
            players.ForEach(player => player.Chips = StartingChips);
            SetRoles(players);
            CreateQueue(ref players);
        }

        public void DoAction(Enums.CommandTypeEnum commandType, int chipsBet, out bool isGameOver, out bool isError, out string actionMessage)
        {
            isGameOver = false;
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    if (!CanPlaceBet(chipsBet, out var message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {chipsBet}.";
                    totalChipsForTurn += chipsBet;
                    previousBet = chipsBet;
                    CurrentPlayer.CurrentBet += chipsBet;
                    CurrentPlayer.Chips -= chipsBet;
                    smallBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    if (!CanPlaceBet(chipsBet, out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {chipsBet}.";
                    totalChipsForTurn += chipsBet;
                    previousBet = chipsBet;
                    CurrentPlayer.CurrentBet += chipsBet;
                    CurrentPlayer.Chips -= chipsBet;
                    bigBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.Check:
                    if (previousCommandType is Enums.CommandTypeEnum.Call or Enums.CommandTypeEnum.Raise)
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
                    if (!CanPlaceBet(previousBet, out message, true))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} called.";
                    totalChipsForTurn += previousBet;
                    CurrentPlayer.CurrentBet += previousBet;
                    CurrentPlayer.Chips -= previousBet;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Raise:
                    if (!CanPlaceBet(chipsBet, out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} raised {chipsBet}.";
                    totalChipsForTurn += chipsBet;
                    previousBet = chipsBet;
                    CurrentPlayer.CurrentBet += chipsBet;
                    CurrentPlayer.Chips -= chipsBet;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }
            previousCommandType = commandType;
            playerQueue.Dequeue();
            if (playerQueue.Count == 1)
            {
                playerQueue.First().Chips += Pot;
                Pot = 0;
                totalChipsForTurn = 0;
                isGameOver = true;
                isError = false;
                actionMessage = "";
                return;
            }
            if (!CurrentPlayer.HasFolded)
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
                    }
                    break;
                case Enums.GameStateEnum.TheTurn:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.TheRiver;
                        SetTheRiver();
                        gameStateChanged = true;
                        UpdatePot();
                    }
                    break;
                case Enums.GameStateEnum.TheRiver:
                    if (playerQueue.All(x => x.HasTakenAction))
                    {
                        GameState = Enums.GameStateEnum.Showdown;
                        gameStateChanged = true;
                        UpdatePot();
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
            var dealer = players.GetRandomElement();
            dealer.IsDealer = true;

            var index = players.IndexOf(dealer);
            index = index + 1 >= players.Count ? 0 : index + 1;
            players[index].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
            index = index + 1 >= players.Count ? 0 : index + 1;
            players[index].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
        }

        private void SetCards(List<PlayerEntity> players)
        {
            var dealer = players.First(x => x.IsDealer);
            var deck = cardPool;
            var shuffled = deck.Shuffle();
            //give to dealer last
            var dealerIndex = players.IndexOf(dealer);
            var startIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
            var dealt = 0;
            while (dealt < players.Count)
            {
                players[startIndex].HoleCards = new List<CardEntity>();
                for (var i = 0; i < MaxHoleCards; i++)
                {
                    var card = shuffled.GetRandomElement();
                    shuffled.Remove(card);
                    players[startIndex].HoleCards.Add(card);
                }
                
                dealt++;
                if (startIndex + 1 >= players.Count)
                    startIndex = 0;
                else
                    startIndex++;
            }

            cardPool = shuffled;
        }
        
        private List<CardEntity> CreateDeck(bool useJokers = false)
        {
            var suits = Enum.GetValues<Enums.CardSuitEnum>();
            var ranks = Enum.GetValues<Enums.CardRankEnum>();
            var deck = new List<CardEntity>();
            foreach (var suit in suits)
            {
                deck.AddRange(ranks.Where(rank => rank != Enums.CardRankEnum.Joker).Select(rank => new CardEntity(suit, rank)));
            }

            if (useJokers)
            {
                deck.Add(new CardEntity(Enums.CardSuitEnum.None, Enums.CardRankEnum.Joker));
                deck.Add(new CardEntity(Enums.CardSuitEnum.None, Enums.CardRankEnum.Joker));
            }

            return deck;
        }
        
        private void CreateQueue(ref List<PlayerEntity> players)
        {
            players = players.OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            for (var i = 1; i < players.Count; i++)
                playerQueue.Enqueue(players[i]);
            playerQueue.Enqueue(players[0]);
            
            CurrentPlayer = playerQueue.Peek();
        }
        
        #endregion
        
        #region Setting community cards
        
        private void SetTheFlop()
        {
            var card1 = cardPool.GetRandomElement();
            cardPool.Remove(card1);
            var card2 = cardPool.GetRandomElement();
            cardPool.Remove(card2);
            var card3 = cardPool.GetRandomElement();
            cardPool.Remove(card3);
            
            CommunityCards[0] = card1;
            CommunityCards[1] = card2;
            CommunityCards[2] = card3;
        }
        
        private void SetTheTurn()
        {
            var card4 = cardPool.GetRandomElement();
            cardPool.Remove(card4);
            CommunityCards[3] = card4;
        }

        private void SetTheRiver()
        {
            var card5 = cardPool.GetRandomElement();
            cardPool.Remove(card5);
            CommunityCards[4] = card5;
        }
        
        #endregion

        public void SetPlayerHand(Guid playerId, CardEntity[] showdownCards)
        {
            playerHands.Add(playerId, showdownCards);
        }
        
        public Guid GetWinner()
        {
            var currentRanking = Enums.HandRankingType.Nothing;
            (Guid PlayerId, CardEntity[] Hand) currentPlayer = (Guid.Empty, []);
            foreach (var hand in playerHands)
            {
                var ranking = HandRankingLogic.GetHandRanking(hand.Value);
                if ((int)ranking < (int)currentRanking)
                {
                    currentRanking = ranking;
                    currentPlayer = (hand.Key, hand.Value);
                    isTie = false;
                }
                else if (ranking == currentRanking)
                {
                    var guid = HandRankingLogic.CompareHands(currentPlayer, (hand.Key, hand.Value), ranking);
                    if (guid == Guid.Empty)
                        isTie = true;
                    else
                    {
                        isTie = false;
                        currentPlayer = currentPlayer.PlayerId == guid ? currentPlayer : (hand.Key, hand.Value);
                    }
                }
            }
            return isTie ? Guid.Empty : currentPlayer.PlayerId;
        }
        
        private bool CanPlaceBet(int chipsBet, out string message, bool isCall = false)
        {
            if (chipsBet <= 0)
            {
                message = "The bet must be greater than 0.";
                return false;
            }
            
            if (previousBet != 0 && chipsBet <= previousBet && !isCall)
            {
                message = "The bet must be greater than the previous bet.";
                return false;
            }
            
            if (chipsBet > CurrentPlayer.Chips)
            {
                message = "Not enough chips.";
                return false;
            }

            message = "";
            return true;
        }
        
        private void UpdatePot()
        {
            Pot += totalChipsForTurn;
            totalChipsForTurn = 0;
            foreach (var player in playerQueue)
                player.CurrentBet = 0;
        }
    }
}