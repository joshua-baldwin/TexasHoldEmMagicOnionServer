using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        private readonly Queue<PlayerEntity> playerQueue = new();
        private List<PlayerEntity> allPlayerList = new();
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private (int Amount, bool IsAllIn, bool IsGreaterThanPrevious) previousBet;
        private (int Amount, int RaiseAmount, bool IsDoublePrevious) lastAllIn;
        private List<CardEntity> cardPool;
        private bool isTie;
        private List<PlayerEntity> allInPlayers = new();
        private List<PlayerEntity> allInPlayersForRound = new();

        public PlayerEntity PreviousPlayer { get; private set; }
        public PlayerEntity CurrentPlayer { get; private set; }
        public List<(Guid, int)> Pots { get; private set; } = [(Guid.Empty, 0)];
        public List<CardEntity> CommunityCards { get; set; } = new();
        public Enums.GameStateEnum GameState { get; private set; }
        public (int RaiseAmount, int TotalBet) CurrentRaise { get; set; }
        public int CurrentRound { get; set; }
        
        public Queue<PlayerEntity> PlayerQueue => playerQueue;

        public void Reset()
        {
            playerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            lastAllIn = (0, 0, false);
            cardPool.Clear();
            isTie = false;
            PreviousPlayer = null;
            CurrentPlayer = null;
            Pots = [(Guid.Empty, 0)];
            CommunityCards.Clear();
            GameState = Enums.GameStateEnum.BlindBet;
            CurrentRound = 0;
        }
        
        public void SetupGame(List<PlayerEntity> players, bool isFirstRound)
        {
            CurrentRound++;
            allPlayerList = players;
            cardPool = CreateDeck();
            if (isFirstRound)
                players.ForEach(player => player.Chips = Constants.StartingChips);
            else
            {
                players.ForEach(player => player.InitializeForNextRound());
                InitializeForNextRound();
            }

            SetRoles(players, isFirstRound);
        }

        public void DoAction(Enums.CommandTypeEnum commandType, int chipsBet, out bool isGameOver, out bool isError, out string actionMessage)
        {
            isGameOver = false;
            isError = false;
            actionMessage = string.Empty;
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    var betAmount = Constants.MinBet / 2;
                    if (!CanPlaceBet((betAmount, 0), out var message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {betAmount}.";
                    previousBet = (betAmount, false, false);
                    CurrentPlayer.AddToCurrentBet(betAmount);
                    CurrentPlayer.Chips -= betAmount;
                    CurrentPlayer.RaiseAmount = betAmount;
                    smallBlindBetDone = true;
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    if (!CanPlaceBet((Constants.MinBet, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} bet {Constants.MinBet}.";
                    previousBet = (Constants.MinBet, false, false);
                    CurrentPlayer.AddToCurrentBet(Constants.MinBet);
                    CurrentPlayer.Chips -= Constants.MinBet;
                    CurrentPlayer.RaiseAmount = Constants.MinBet;
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
                    if (playerQueue.Count(x => x.HasFolded) == playerQueue.Count + allInPlayers.Count - 1)
                    {
                        isGameOver = true;
                        GameState = Enums.GameStateEnum.GameOver;
                    }
                    break;
                case Enums.CommandTypeEnum.Call:
                    int callAmount;
                    //check if last raiser is able to match the last person that went all in
                    if (CurrentRaise.RaiseAmount != 0 && CurrentPlayer.LastCommand == Enums.CommandTypeEnum.Raise && CurrentPlayer.GetCurrentBet() < lastAllIn.RaiseAmount)
                        callAmount = CurrentRaise.RaiseAmount;
                    //match the last raise
                    else if (previousBet.Amount < CurrentRaise.TotalBet)
                        callAmount = CurrentRaise.TotalBet;
                    //match the last all in
                    else if (lastAllIn.Amount > previousBet.Amount)
                        callAmount = lastAllIn.Amount;
                    //match the last bet
                    else
                        callAmount = previousBet.Amount;

                    if (!CanPlaceBet((callAmount, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} called.";
                    previousBet = (callAmount, false, false);
                    CurrentPlayer.AddToCurrentBet(callAmount);
                    CurrentPlayer.Chips -= callAmount;
                    CurrentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Raise:
                    betAmount = chipsBet + previousBet.Amount;
                    if (!CanPlaceBet((betAmount, chipsBet), out message, true))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} raised {chipsBet}.";
                    previousBet = (betAmount, false, betAmount > CurrentRaise.TotalBet);
                    CurrentPlayer.AddToCurrentBet(betAmount);
                    CurrentPlayer.Chips -= betAmount;
                    CurrentPlayer.RaiseAmount = chipsBet;
                    if (betAmount > CurrentRaise.TotalBet)
                    {
                        foreach (var player in playerQueue)
                            player.HasTakenAction = false;
                    }
                    
                    CurrentPlayer.HasTakenAction = true;
                    CurrentRaise = CurrentRaise.RaiseAmount == 0 
                        ? (0, betAmount)
                        : (chipsBet, betAmount);
                    break;
                case Enums.CommandTypeEnum.AllIn:
                    allInPlayersForRound.Add(CurrentPlayer);
                    foreach (var player in playerQueue.Where(x => x.Id != CurrentPlayer.Id && !x.IsAllIn))
                        player.AddNewCurrentBet(0);
                    actionMessage = $"{CurrentPlayer.Name} went all in.";
                    
                    var isRaise = previousBet.Amount != 0 && CurrentPlayer.Chips - CurrentRaise.TotalBet > CurrentRaise.RaiseAmount;
                    lastAllIn = (CurrentPlayer.Chips, isRaise ? CurrentPlayer.Chips - CurrentRaise.TotalBet : 0, isRaise);

                    CurrentPlayer.HasTakenAction = true;
                    if (isRaise)
                    {
                        CurrentRaise = (CurrentPlayer.Chips - CurrentRaise.TotalBet, CurrentPlayer.Chips);
                        foreach (var player in playerQueue)
                            player.HasTakenAction = false;
                    }
                    else if (CurrentPlayer.Chips - CurrentRaise.TotalBet > CurrentRaise.RaiseAmount)
                        CurrentRaise = (CurrentPlayer.Chips - CurrentRaise.TotalBet, CurrentPlayer.Chips);

                    previousBet = (CurrentPlayer.Chips, true, isRaise);
                    
                    CurrentPlayer.AddToCurrentBet(CurrentPlayer.Chips);
                    CurrentPlayer.Chips = 0;
                    CurrentPlayer.IsAllIn = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }

            PreviousPlayer = playerQueue.Dequeue();
            PreviousPlayer.LastCommand = commandType;
            if (!CurrentPlayer.HasFolded && (CurrentPlayer.Chips > 0 || CurrentPlayer.Chips == 0 && CurrentPlayer.IsAllIn))
                playerQueue.Enqueue(CurrentPlayer);
            CurrentPlayer = playerQueue.Peek();
            if (playerQueue.Any(x => !x.IsAllIn))
            {
                while (CurrentPlayer.IsAllIn)
                {
                    CurrentPlayer = playerQueue.Dequeue();
                    playerQueue.Enqueue(CurrentPlayer);
                    CurrentPlayer = playerQueue.Peek();
                }
            }

            UpdateGameState();
        }

        private void InitializeForNextRound()
        {
            playerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            lastAllIn = (0, 0, false);
            isTie = false;
            allInPlayers.Clear();
            PreviousPlayer = null;
            CurrentPlayer = null;
            Pots = [(Guid.Empty, 0)];
            CommunityCards.Clear();
            GameState = Enums.GameStateEnum.BlindBet;
            CurrentRaise = (0, 0);
        }

        private void UpdateGameState()
        {
            var gameStateChanged = false;
            if (playerQueue.Count == 1 || playerQueue.All(x => x.IsAllIn) || (playerQueue.Count(x => x.IsAllIn) == playerQueue.Count - 1 && playerQueue.All(x => x.HasTakenAction)))
            {
                if (GameState == Enums.GameStateEnum.GameOver)
                {
                    gameStateChanged = true;
                    UpdatePot();    
                }
                else
                {
                    GameState = Enums.GameStateEnum.Showdown;
                    SetRemainingCards();
                    gameStateChanged = true;
                    UpdatePot();
                }
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
                            UpdatePot();
                        }

                        break;
                    case Enums.GameStateEnum.PreFlop:
                        if (playerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheFlop;
                            SetTheFlop();
                            gameStateChanged = true;
                            UpdatePot();
                        }

                        break;
                    case Enums.GameStateEnum.TheFlop:
                        if (playerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheTurn;
                            SetTheTurn();
                            gameStateChanged = true;
                            UpdatePot();
                        }

                        break;
                    case Enums.GameStateEnum.TheTurn:
                        if (playerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheRiver;
                            SetTheRiver();
                            gameStateChanged = true;
                            UpdatePot();
                        }

                        break;
                    case Enums.GameStateEnum.TheRiver:
                        if (playerQueue.All(x => x.HasTakenAction || x.IsAllIn))
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
            
            var players = playerQueue.Where(x => !x.IsAllIn).OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            allInPlayers = playerQueue.Where(x => x.IsAllIn).ToList();
            playerQueue.Clear();
            for (var i = 1; i < players.Count; i++)
                playerQueue.Enqueue(players[i]);
            
            if (players.Count > 0)
            {
                playerQueue.Enqueue(players[0]);
                CurrentPlayer = playerQueue.Peek();
                foreach (var player in playerQueue)
                {
                    player.HasTakenAction = false;
                    player.LastCommand = 0;
                }
            }

            lastAllIn = (0, 0, false);
            previousBet = (0, false, false);
            CurrentRaise = (0, 0);
            allInPlayersForRound.Clear();
        }

        #region Setup
        
        private void SetRoles(List<PlayerEntity> players, bool isFirstRound)
        {
            if (isFirstRound)
            {
                var dealer = players.GetRandomElement();
                dealer.IsDealer = true;

                var index = players.IndexOf(dealer);
                index = index + 1 >= players.Count ? 0 : index + 1;
                players[index].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
                index = index + 1 >= players.Count ? 0 : index + 1;
                players[index].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
            }
            else
            {
                var dealer = players.First(x => x.IsDealer);
                dealer.IsDealer = false;
                var dealerIndex = players.IndexOf(dealer);
                dealerIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
                players[dealerIndex].PlayerRole = Enums.PlayerRoleEnum.None;
                players[dealerIndex].IsDealer = true;
                dealerIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
                players[dealerIndex].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
                dealerIndex = dealerIndex + 1 >= players.Count ? 0 : dealerIndex + 1;
                players[dealerIndex].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
            }
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
                for (var i = 0; i < Constants.MaxHoleCards; i++)
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
                for (var i = 0; i < Constants.JokerCount; i++)
                    deck.Add(new CardEntity(Enums.CardSuitEnum.None, Enums.CardRankEnum.Joker));
            }

            return deck;
        }
        
        public void CreateQueue(List<PlayerEntity> players)
        {
            var playerList = players.OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            for (var i = 1; i < playerList.Count; i++)
                playerQueue.Enqueue(playerList[i]);
            playerQueue.Enqueue(playerList[0]);
            
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

        public List<WinningHandEntity> DoShowdown()
        {
            var winnerList = new List<WinningHandEntity>();
            var showdownDictionary = new Dictionary<Guid, List<PlayerEntity>>();
            while (Pots.Count > 0)
            {
                var firstPot = Pots[0];
                var potPlayer = allPlayerList.FirstOrDefault(x => x.Id == firstPot.Item1);
                var eligiblePlayers = potPlayer == null
                    ? allPlayerList.Where(x => !x.HasFolded && !x.IsAllIn).ToList()
                    : allPlayerList.Where(x => !x.HasFolded && (!x.IsAllIn || showdownDictionary.ContainsKey(x.Id))).Concat([potPlayer]).ToList();
                showdownDictionary.Add(potPlayer == null ? Guid.Empty : potPlayer.Id, eligiblePlayers);
                var winner = GetWinner(showdownDictionary[firstPot.Item1]);
                winnerList.Add(winner);
                Pots.RemoveAt(0);
            }
            
            return winnerList;
        }
        
        private WinningHandEntity GetWinner(List<PlayerEntity> showdownPlayers)
        {
            var winningHand = new WinningHandEntity();
            if (showdownPlayers.Count == 1)
            {
                var player = showdownPlayers.First();
                var hand = player.HoleCards.Concat(CommunityCards).ToList();
                var rank = HandRankingLogic.GetHandRanking(hand);
                player.BestHand = new BestHandEntity { Cards = hand.Where(x => x.IsFinalHand).ToList(), HandRanking = rank };
                winningHand.HandRanking = rank;
                winningHand.Winner = player;
                winningHand.Cards = player.BestHand.Cards;
                winningHand.Winner.Chips += Pots[0].Item2;
                winningHand.PotToWinner = Pots[0].Item2;
                return winningHand;
            }
            
            foreach (var player in showdownPlayers)
            {
                Enums.HandRankingType ranking;
                if (player.BestHand == null)
                {
                    var hand = player.HoleCards.Concat(CommunityCards).ToList();
                    ranking = HandRankingLogic.GetHandRanking(hand);
                    player.BestHand = new BestHandEntity { Cards = hand.Where(x => x.IsFinalHand).ToList(), HandRanking = ranking };
                }
                else
                    ranking = player.BestHand.HandRanking;

                if ((int)ranking < (int)winningHand.HandRanking)
                {
                    winningHand.HandRanking = ranking;
                    winningHand.Winner = player;
                    winningHand.Cards = player.BestHand.Cards;
                    isTie = false;
                }
                else if (ranking == winningHand.HandRanking)
                {
                    var guid = HandRankingLogic.CompareHands((winningHand.Winner.Id, winningHand.Cards), (player.Id, player.BestHand.Cards), ranking);
                    if (guid == Guid.Empty)
                    {
                        isTie = true;
                        winningHand.TiedWith.Add(winningHand.Winner);
                        winningHand.TiedWith.Add(player);
                        winningHand.Winner = null;
                    }
                    else
                    {
                        isTie = false;
                        if (winningHand.Winner.Id == guid)
                            continue;
                        
                        winningHand.Winner = player;
                        winningHand.HandRanking = ranking;
                        winningHand.Cards = player.BestHand.Cards;
                        winningHand.TiedWith.Clear();
                    }
                }
            }

            if (isTie)
            {
                //TODO figure out how to split evenly
                var split = Pots[0].Item2 / 2;
                winningHand.TiedWith.ForEach(player => player.Chips += split);
                winningHand.PotToTiedWith = split;
            }
            else
            {
                winningHand.Winner.Chips += Pots[0].Item2;
                winningHand.PotToWinner = Pots[0].Item2;
            }

            return winningHand;
        }
        
        private bool CanPlaceBet((int BetAmount, int RaiseAmount) chipsBet, out string message, bool isRaise = false)
        {
            if (chipsBet.BetAmount <= 0)
            {
                message = "The bet must be greater than 0.\n0以上ベットしないといけない。";
                return false;
            }

            if (isRaise && chipsBet.RaiseAmount < Constants.RaiseAmount)
            {
                message = $"The minimum raise is {Constants.RaiseAmount}.\nミニマムレイズは{Constants.RaiseAmount}。";
                return false;
            }
            
            if (chipsBet.BetAmount > CurrentPlayer.Chips)
            {
                message = "Not enough chips.\nチップが足りない。";
                return false;
            }

            message = "";
            return true;
        }

        private void UpdatePot()
        {
            var allPlayers = playerQueue.ToList();
            while (allPlayers.Any(x => x.IsAllIn))
            {
                var minNotAllIn = allPlayers.Where(x => !x.IsAllIn && x.GetCurrentBet() != 0).MinBy(x => x.GetCurrentBet());
                var minAllIn = allPlayers.Where(x => x.IsAllIn).MinBy(x => x.GetCurrentBet());
                var playerWithMinimumBet = minNotAllIn == null || minAllIn.GetCurrentBet() < minNotAllIn.GetCurrentBet() || minNotAllIn.LastCommand == Enums.CommandTypeEnum.Call ? minAllIn : minNotAllIn;
                var firstAllInPlayer = allPlayers.Where(x => x.IsAllIn).MinBy(x => x.GetCurrentBet());
                var minAllInPlayers = allPlayers.Where(x => x.IsAllIn && x.GetCurrentBet() == firstAllInPlayer.GetCurrentBet()).ToList();
                if (Pots.Count == 0 || Pots[0].Item1 != Guid.Empty)
                    Pots.Insert(0, (firstAllInPlayer.Id, playerWithMinimumBet.GetCurrentBet() * allPlayers.Count));
                else
                {
                    var item = Pots[0];
                    item.Item1 = firstAllInPlayer.Id;
                    if (playerWithMinimumBet.IsAllIn)
                        item.Item2 += playerWithMinimumBet.GetCurrentBet() * allPlayers.Count;
                    else
                        item.Item2 = playerWithMinimumBet.GetCurrentBet() * allPlayers.Count;
                    Pots[0] = item;
                }

                var minBet = playerWithMinimumBet.GetCurrentBet();
                allPlayers.ForEach(player =>
                {
                    player.SubtractFromCurrentBet(minBet);
                });

                foreach (var player in minAllInPlayers)
                {
                    allPlayers.Remove(player);
                    allPlayers.ForEach(x =>
                    {
                        if (!x.IsAllIn)
                            x.RemoveCurrentBet();
                    });
                    //TODO remove extra currentbets
                }
            }

            var sidePot = 0;
            if (allPlayers.Count > 1)
            {
                foreach (var player in allPlayers)
                {
                    sidePot += player.GetCurrentBet();
                    player.ResetCurrentBets();
                }
                
                if (Pots.Count == 0 || Pots[0].Item1 != Guid.Empty)
                    Pots.Insert(0, (Guid.Empty, sidePot));
                else
                {
                    var item = Pots[0];
                    item.Item2 += sidePot;
                    Pots[0] = item;
                }
            }

            if (GameState != Enums.GameStateEnum.PreFlop)
                previousBet = (0, false, false);
        }
    }
}