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
        public const int SmallBlindBet = 1;
        public const int BigBlindBet = 2;
        public const int JokerCount = 2;
        private readonly Queue<PlayerEntity> playerQueue = new();
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private int previousBet;
        private List<CardEntity> cardPool;
        private bool isTie;
        
        public PlayerEntity PreviousPlayer { get; private set; }
        public PlayerEntity CurrentPlayer { get; private set; }
        public List<(Guid, int)> Pots { get; private set; } = new();
        public List<CardEntity> CommunityCards { get; } = new();
        public Enums.GameStateEnum GameState { get; private set; }
        
        public Queue<PlayerEntity> PlayerQueue => playerQueue;

        public void Reset()
        {
            playerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = 0;
            cardPool.Clear();
            isTie = false;
            PreviousPlayer = null;
            CurrentPlayer = null;
            Pots = [];
            for (var i = 0; i < CommunityCards.Count; i++)
                CommunityCards[i] = null;
            GameState = Enums.GameStateEnum.BlindBet;
        }
        public void SetupGame(List<PlayerEntity> players)
        {
            cardPool = CreateDeck();
            players.ForEach(player => player.Chips = StartingChips);
            SetRoles(players);
        }

        public void DoAction(Enums.CommandTypeEnum commandType, int chipsBet, out bool isGameOver, out bool isError, out string actionMessage)
        {
            isGameOver = false;
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    if (!CanPlaceBet(SmallBlindBet, out var message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {SmallBlindBet}.";
                    previousBet = SmallBlindBet;
                    CurrentPlayer.CurrentBet = SmallBlindBet;
                    CurrentPlayer.Chips -= SmallBlindBet;
                    smallBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    if (!CanPlaceBet(BigBlindBet, out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {BigBlindBet}.";
                    previousBet = BigBlindBet;
                    CurrentPlayer.CurrentBet = BigBlindBet;
                    CurrentPlayer.Chips -= BigBlindBet;
                    bigBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.Check:
                    var canCheck = (PreviousPlayer.LastCommand is not Enums.CommandTypeEnum.Call &&
                                    CurrentPlayer.LastCommand is not Enums.CommandTypeEnum.Raise)
                                   ||
                                   (GameState == Enums.GameStateEnum.PreFlop &&
                                    CurrentPlayer.PlayerRole == Enums.PlayerRoleEnum.BigBlind &&
                                    playerQueue.All(x => x.LastCommand != Enums.CommandTypeEnum.Raise));
                    if (!canCheck)
                    {
                        actionMessage = "You can't check because a bet has been placed.\n誰かがベットしたのでチェックできない。";
                        isError = true;
                        return;
                    }
                    
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Fold:
                    actionMessage = $"{CurrentPlayer.Name} folded.";
                    CurrentPlayer.HasFolded = true;
                    if (playerQueue.Count(x => x.HasFolded) == playerQueue.Count - 1)
                    {
                        //TODO TODO TODO TODO
                        //playerQueue.First(x => !x.HasFolded).Chips += Pots.Value;
                        Pots = [];
                        isGameOver = true;
                        isError = false;
                        return;
                    }
                    break;
                case Enums.CommandTypeEnum.Call:
                    if (!CanPlaceBet(previousBet, out message, true))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} called.";
                    CurrentPlayer.CurrentBet = previousBet;
                    CurrentPlayer.Chips -= previousBet;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Raise:
                    if (!CanPlaceBet(chipsBet, out message, isRaise: true))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} raised {chipsBet}.";
                    previousBet = chipsBet;
                    CurrentPlayer.CurrentBet = chipsBet;
                    CurrentPlayer.Chips -= chipsBet;
                    foreach (var player in playerQueue)
                        player.HasTakenAction = false;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.AllIn:
                    actionMessage = $"{CurrentPlayer.Name} went all in.";
                    CurrentPlayer.CurrentBet = CurrentPlayer.Chips;
                    CurrentPlayer.Chips = 0;
                    CurrentPlayer.IsAllIn = true;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }
            PreviousPlayer = playerQueue.Dequeue();
            PreviousPlayer.LastCommand = commandType;
            if (!CurrentPlayer.HasFolded)
                playerQueue.Enqueue(CurrentPlayer);
            CurrentPlayer = playerQueue.Peek();
            while (CurrentPlayer.IsAllIn)
            {
                CurrentPlayer = playerQueue.Dequeue();
                playerQueue.Enqueue(CurrentPlayer);
                CurrentPlayer = playerQueue.Peek();
            }
            UpdateGameState();
            actionMessage = "";
            isError = false;
        }

        private void UpdateGameState()
        {
            var gameStateChanged = false;
            if (playerQueue.Count(x => x.IsAllIn) == playerQueue.Count - 1)
            {
                GameState = Enums.GameStateEnum.Showdown;
                SetRemainingCards();
                gameStateChanged = true;
                UpdatePot();
            }
            else
            {
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
            }

            if (!gameStateChanged || (gameStateChanged && GameState == Enums.GameStateEnum.PreFlop))
                return;
            
            //TODO refactor CreateQueue method
            //reset queue if people checked
            var players = playerQueue.OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            playerQueue.Clear();
            for (var i = 1; i < players.Count; i++)
                playerQueue.Enqueue(players[i]);
            playerQueue.Enqueue(players[0]);
            
            CurrentPlayer = playerQueue.Peek();

            foreach (var player in playerQueue)
            {
                player.HasTakenAction = false;
                player.LastCommand = 0;
            }
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
            foreach (var suit in suits.Where(x => x != Enums.CardSuitEnum.None))
            {
                deck.AddRange(ranks.Where(rank => rank != Enums.CardRankEnum.Joker).Select(rank => new CardEntity(suit, rank)));
            }

            if (useJokers)
            {
                for (var i = 0; i < JokerCount; i++)
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
            
            CommunityCards.Add(card1);
            CommunityCards.Add(card2);
            CommunityCards.Add(card3);
        }
        
        private void SetTheTurn()
        {
            var card4 = cardPool.GetRandomElement();
            cardPool.Remove(card4);
            CommunityCards.Add(card4);
        }

        private void SetTheRiver()
        {
            var card5 = cardPool.GetRandomElement();
            cardPool.Remove(card5);
            CommunityCards.Add(card5);
        }

        private void SetRemainingCards()
        {
            while (CommunityCards.Count < 5)
            {
                var card = cardPool.GetRandomElement();
                cardPool.Remove(card);
                CommunityCards.Add(card);
            }
        }
        
        #endregion

        public List<(List<Guid>, Enums.HandRankingType)> DoShowdown()
        {
            var winnerList = new List<(List<Guid>, Enums.HandRankingType)>();
            var loserList = new List<Guid>();
            while (Pots.Count > 0)
            {
                var firstPot = Pots[0];
                var showdownPlayers = playerQueue.Where(x => !x.IsAllIn).Concat([playerQueue.First(x => x.Id == firstPot.Item1)]).ToList();
                showdownPlayers = showdownPlayers.Concat(playerQueue.Where(x => winnerList.SelectMany(y => y.Item1).Contains(x.Id))).DistinctBy(x => x.Id).ToList();
                showdownPlayers.RemoveAll(x => loserList.Contains(x.Id));
                var winner = GetWinner(showdownPlayers);
                winnerList.Add(winner);
                loserList.Add(showdownPlayers.First(x => x.Id != winner.Item1.First()).Id);
                Pots.RemoveAt(0);
            }
            return winnerList;
        }
        
        private (List<Guid>, Enums.HandRankingType) GetWinner(List<PlayerEntity> showdownPlayers)
        {
            var tieIds = new List<Guid>();
            var winningHand = Enums.HandRankingType.Nothing;
            (Guid PlayerId, CardEntity[] Hand) currentPlayer = (Guid.Empty, []);
            foreach (var player in showdownPlayers)
            {
                var hand = player.HoleCards.Concat(CommunityCards).ToArray();
                var ranking = HandRankingLogic.GetHandRanking(hand);
                var finalHand = hand.Where(x => x.IsFinalHand).ToArray();
                if ((int)ranking < (int)winningHand)
                {
                    winningHand = ranking;
                    currentPlayer = (player.Id, finalHand);
                    isTie = false;
                }
                else if (ranking == winningHand)
                {
                    var guid = HandRankingLogic.CompareHands(currentPlayer, (player.Id, finalHand), ranking);
                    if (guid == Guid.Empty)
                    {
                        isTie = true;
                        tieIds.Add(currentPlayer.PlayerId);
                        tieIds.Add(player.Id);
                    }
                    else
                    {
                        isTie = false;
                        currentPlayer = currentPlayer.PlayerId == guid ? currentPlayer : (player.Id, finalHand);
                    }
                }
            }

            if (isTie)
            {
                //TODO figure out how to split evenly
                //TODO TODO TODO TODO
                //var split = Pots.Value / 2;
                //playerQueue.First(x => x.Id == tieIds[0]).Chips += split;
                //playerQueue.First(x => x.Id == tieIds[1]).Chips += split;
                //Pots = [];
                return (tieIds, winningHand);
            }

            var winner = showdownPlayers.First(x => x.Id == currentPlayer.PlayerId);
            //TODO TODO TODO TODO
            //winner.Chips += Pots.Value;
            //Pots = [];
            return ([currentPlayer.PlayerId], winningHand);
        }
        
        private bool CanPlaceBet(int chipsBet, out string message, bool isCall = false, bool isRaise = false)
        {
            if (chipsBet <= 0)
            {
                message = "The bet must be greater than 0.\n0以上ベットしないといけない。";
                return false;
            }
            
            if (previousBet != 0 && chipsBet <= previousBet && !isCall)
            {
                message = "The bet must be greater than the previous bet.\nさっきのベットより高いベットをしないといけない。";
                return false;
            }

            if (isRaise && chipsBet < BigBlindBet)
            {
                message = $"The minimum bet is {BigBlindBet}.\n最低限のベットは{BigBlindBet}。";
                return false;
            }

            if (isRaise && chipsBet <= previousBet * 2)
            {
                message = $"You must bet double the previous bet of {previousBet}.\nさっきの{previousBet}の倍をベットしないといけない。";
            }
            
            if (chipsBet > CurrentPlayer.Chips)
            {
                message = "Not enough chips.\nチップが足りない。";
                return false;
            }

            message = "";
            return true;
        }
        
        private void UpdatePot()
        {
            MainPot += totalChipsForTurn;
            totalChipsForTurn = 0;
            previousBet = 0;
            foreach (var player in playerQueue)
                player.CurrentBet = 0;
        }
    }
}