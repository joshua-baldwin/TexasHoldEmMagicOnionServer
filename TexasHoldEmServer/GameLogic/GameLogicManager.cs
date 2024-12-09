using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Utilities;

namespace TexasHoldEmServer.GameLogic
{
    public class GameLogicManager
    {
        private readonly List<PlayerEntity> allInPlayersForRound = [];
        private List<PlayerEntity> allPlayerList = [];
        private List<PlayerEntity> allInPlayers = [];
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private (int Amount, bool IsAllIn, bool IsGreaterThanPrevious) previousBet;
        private List<CardEntity> cardPool;
        private bool isTie;
        private int maxBetForTurn;
        private (int RaiseAmount, int TotalBet) currentRaise;

        public Queue<PlayerEntity> PlayerQueue { get; } = new();
        public PlayerEntity PreviousPlayer { get; private set; }
        public PlayerEntity CurrentPlayer { get; private set; }
        public List<PotEntity> Pots { get; private set; } = [new (Guid.Empty, 0, 0, null)];
        public List<CardEntity> CommunityCards { get; set; } = new();
        public Enums.GameStateEnum GameState { get; private set; }
        public int CurrentRound { get; private set; }

        public void Reset()
        {
            PlayerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            cardPool.Clear();
            isTie = false;
            PreviousPlayer = null;
            CurrentPlayer = null;
            Pots = [new PotEntity(Guid.Empty, 0, 0, null)];
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
            PotEntity potEntity;
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
                    CurrentPlayer.CurrentBet += betAmount;
                    CurrentPlayer.Chips -= betAmount;
                    CurrentPlayer.RaiseAmount = betAmount;
                    smallBlindBetDone = true;
                    
                    if (Pots.Count == 0 || Pots[0].GoesToPlayer != Guid.Empty)
                        Pots.Insert(0, new PotEntity(Guid.Empty, betAmount, 0, null));
                    else
                    {
                        potEntity = Pots[0];
                        potEntity.PotAmount += betAmount;
                        Pots[0] = potEntity;
                    }
                    
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
                    CurrentPlayer.CurrentBet += Constants.MinBet;
                    CurrentPlayer.Chips -= Constants.MinBet;
                    CurrentPlayer.RaiseAmount = Constants.MinBet;
                    bigBlindBetDone = true;
                    maxBetForTurn = Constants.MinBet;
                    
                    potEntity = Pots[0];
                    potEntity.PotAmount += Constants.MinBet;
                    Pots[0] = potEntity;
                    
                    break;
                case Enums.CommandTypeEnum.Check:
                    var canCheck = (PreviousPlayer.LastCommand is not Enums.CommandTypeEnum.Call &&
                                    CurrentPlayer.LastCommand is not Enums.CommandTypeEnum.Raise)
                                   ||
                                   (GameState == Enums.GameStateEnum.PreFlop &&
                                    CurrentPlayer.PlayerRole == Enums.PlayerRoleEnum.BigBlind &&
                                    PlayerQueue.All(x => x.LastCommand != Enums.CommandTypeEnum.Raise));
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
                    if (PlayerQueue.Count(x => x.HasFolded) == PlayerQueue.Count + allInPlayers.Count - 1)
                    {
                        isGameOver = true;
                        GameState = Enums.GameStateEnum.GameOver;
                    }
                    break;
                case Enums.CommandTypeEnum.Call:
                    var callAmount = maxBetForTurn - CurrentPlayer.CurrentBet;

                    if (!CanPlaceBet((callAmount, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{CurrentPlayer.Name} called.";
                    previousBet = (callAmount, false, false);
                    CurrentPlayer.CurrentBet += callAmount;
                    CurrentPlayer.Chips -= callAmount;
                    CurrentPlayer.HasTakenAction = true;

                    DistributeBetAmountToPots(callAmount);
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
                    previousBet = (betAmount, false, betAmount > currentRaise.TotalBet);
                    CurrentPlayer.CurrentBet += betAmount;
                    CurrentPlayer.Chips -= betAmount;
                    CurrentPlayer.RaiseAmount = chipsBet;
                    if (betAmount > currentRaise.TotalBet)
                    {
                        foreach (var player in PlayerQueue)
                            player.HasTakenAction = false;
                    }
                    
                    CurrentPlayer.HasTakenAction = true;
                    currentRaise = currentRaise.RaiseAmount == 0 
                        ? (0, betAmount)
                        : (chipsBet, betAmount);
                    maxBetForTurn = betAmount;
                    
                    DistributeBetAmountToPots(betAmount);
                    break;
                case Enums.CommandTypeEnum.AllIn:
                    allInPlayersForRound.Add(CurrentPlayer);
                    actionMessage = $"{CurrentPlayer.Name} went all in.";
                    
                    var isRaise = previousBet.Amount != 0 && CurrentPlayer.Chips - currentRaise.TotalBet > currentRaise.RaiseAmount;

                    CurrentPlayer.HasTakenAction = true;
                    if (isRaise)
                    {
                        currentRaise = (CurrentPlayer.Chips - currentRaise.TotalBet, CurrentPlayer.Chips);
                        foreach (var player in PlayerQueue)
                            player.HasTakenAction = false;
                    }
                    else if (CurrentPlayer.Chips - currentRaise.TotalBet > currentRaise.RaiseAmount)
                        currentRaise = (CurrentPlayer.Chips - currentRaise.TotalBet, CurrentPlayer.Chips);

                    previousBet = (CurrentPlayer.Chips, true, isRaise);
                    
                    CurrentPlayer.CurrentBet += CurrentPlayer.Chips;
                    CurrentPlayer.Chips = 0;
                    CurrentPlayer.IsAllIn = true;
                    if (CurrentPlayer.CurrentBet > maxBetForTurn)
                        maxBetForTurn = CurrentPlayer.CurrentBet;

                    RecalculatePots();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }

            PreviousPlayer = PlayerQueue.Dequeue();
            PreviousPlayer.LastCommand = commandType;
            if (!CurrentPlayer.HasFolded && (CurrentPlayer.Chips > 0 || CurrentPlayer.Chips == 0 && CurrentPlayer.IsAllIn))
                PlayerQueue.Enqueue(CurrentPlayer);
            CurrentPlayer = PlayerQueue.Peek();
            if (PlayerQueue.Any(x => !x.IsAllIn))
            {
                while (CurrentPlayer.IsAllIn)
                {
                    CurrentPlayer = PlayerQueue.Dequeue();
                    PlayerQueue.Enqueue(CurrentPlayer);
                    CurrentPlayer = PlayerQueue.Peek();
                }
            }

            UpdateGameState();
        }

        private void DistributeBetAmountToPots(int callAmount)
        {
            PotEntity potEntity;
            var potIndex = Pots.Count - 1;
            while (callAmount > 0 && potIndex >= 0 && Pots[0].GoesToPlayer != Guid.Empty)
            {
                var allInAmount = PlayerQueue.FirstOrDefault(x => x.Id == Pots[potIndex].GoesToPlayer)?.CurrentBet;
                potEntity = Pots[potIndex];
                if (allInAmount.HasValue && allInAmount.Value != 0)
                {
                    var toPot = callAmount - allInAmount.Value > 0 ? callAmount - allInAmount.Value : callAmount;
                    potEntity.PotAmount += toPot;
                    callAmount -= toPot;
                }
                else
                {
                    potEntity.PotAmount += callAmount;
                    callAmount -= callAmount;
                }

                Pots[potIndex] = potEntity;
                potIndex--;
            }

            if (callAmount <= 0)
                return;
            
            if (Pots[0].GoesToPlayer != Guid.Empty)
                Pots.Insert(0, new PotEntity(Guid.Empty, callAmount, 0, null));
            else
            {
                potEntity = Pots[0];
                potEntity.PotAmount += callAmount;
                Pots[0] = potEntity;
            }
        }

        private void RecalculatePots()
        {
            PotEntity potEntity;
            //if a second person goes all in for a lower amount need to create a new pot and insert to the bottom and recalculate pot amounts
            var createNewPot = Pots.Any(x => x.AllInAmount > CurrentPlayer.CurrentBet);
            if (Pots.Count == 0 || createNewPot)
            {
                var index = Pots.IndexOf(Pots.First(x => x.AllInAmount > CurrentPlayer.CurrentBet));
                var eligiblePlayers = PlayerQueue.Where(x => !x.IsAllIn).Concat(allInPlayersForRound.Where(x => x.CurrentBet >= CurrentPlayer.CurrentBet)).ToList();
                Pots.Insert(index + 1, new PotEntity(CurrentPlayer.Id, CurrentPlayer.CurrentBet, CurrentPlayer.CurrentBet, eligiblePlayers));
                        
                var amounts = PlayerQueue.Where(x => x.HasTakenAction).OrderByDescending(x => x.CurrentBet).Select(x => x.CurrentBet).ToList();
                var potIndex2 = Pots.Count - 1;
                while (amounts.Count > 0)
                {
                    var toTake = amounts.Last() * amounts.Count;
                    potEntity = Pots[potIndex2];
                    potEntity.PotAmount = toTake;
                    Pots[potIndex2] = potEntity;
                    potIndex2--;
                            
                    for (var i = 0; i < amounts.Count; i++)
                        amounts[i] -= amounts.Last();

                    amounts.RemoveAll(x => x == 0);
                }
            }
            else
            {
                var allInBet = CurrentPlayer.CurrentBet;
                potEntity = Pots[0];
                potEntity.GoesToPlayer = CurrentPlayer.Id;
                potEntity.PotAmount += allInBet;
                potEntity.AllInAmount = CurrentPlayer.CurrentBet;
                potEntity.EligiblePlayers = PlayerQueue.Where(x => !x.IsAllIn).Concat(allInPlayersForRound.Where(x => x.CurrentBet >= CurrentPlayer.CurrentBet)).ToList();
                Pots[0] = potEntity;
                        
                var sidePot = 0;
                foreach (var player in PlayerQueue.Where(x => x.Id != CurrentPlayer.Id))
                {
                    if (allInBet >= player.CurrentBet)
                        continue;
                            
                    var extra = player.CurrentBet - allInBet;
                    potEntity = Pots[0];
                    potEntity.PotAmount -= extra;
                    Pots[0] = potEntity;
                    sidePot += extra;
                }

                if (sidePot != 0)
                {
                    if (Pots[0].GoesToPlayer != Guid.Empty)
                        Pots.Insert(0, new PotEntity(Guid.Empty, sidePot, 0, PlayerQueue.Where(x => !x.IsAllIn && !x.HasFolded).ToList()));
                    else
                    {
                        potEntity = Pots[0];
                        potEntity.PotAmount += sidePot;
                        Pots[0] = potEntity;
                    }   
                }
            }

            //if another player goes all in, if they were placed in the eligible list beforehand and their all in is a lower amount, remove them from the list
            foreach (var pot in Pots)
            {
                if (pot.GoesToPlayer == Guid.Empty)
                    pot.EligiblePlayers.RemoveAll(x => x.IsAllIn);
                else
                {
                    var allInPlayer = PlayerQueue.FirstOrDefault(x => x.Id == pot.GoesToPlayer);
                    var self = pot.EligiblePlayers?.FirstOrDefault(x => x.Id == CurrentPlayer.Id);
                    if (self?.CurrentBet < allInPlayer?.CurrentBet)
                        pot.EligiblePlayers.RemoveAll(x => x.Id == self.Id);
                }
            }
        }

        private void InitializeForNextRound()
        {
            PlayerQueue.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            isTie = false;
            allInPlayers.Clear();
            PreviousPlayer = null;
            CurrentPlayer = null;
            Pots = [new PotEntity(Guid.Empty, 0, 0, null)];
            CommunityCards.Clear();
            GameState = Enums.GameStateEnum.BlindBet;
            currentRaise = (0, 0);
        }

        private void UpdateGameState()
        {
            var gameStateChanged = false;
            if (PlayerQueue.Count == 1 || PlayerQueue.All(x => x.IsAllIn) || (PlayerQueue.Count(x => x.IsAllIn) == PlayerQueue.Count - 1 && PlayerQueue.All(x => x.HasTakenAction)))
            {
                if (GameState == Enums.GameStateEnum.GameOver)
                    gameStateChanged = true;
                else
                {
                    GameState = Enums.GameStateEnum.Showdown;
                    SetRemainingCards();
                    gameStateChanged = true;
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
                            SetCards(PlayerQueue.ToList());
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.PreFlop:
                        if (PlayerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheFlop;
                            SetTheFlop();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheFlop:
                        if (PlayerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheTurn;
                            SetTheTurn();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheTurn:
                        if (PlayerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.TheRiver;
                            SetTheRiver();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheRiver:
                        if (PlayerQueue.All(x => x.HasTakenAction || x.IsAllIn))
                        {
                            GameState = Enums.GameStateEnum.Showdown;
                            gameStateChanged = true;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!gameStateChanged || (gameStateChanged && GameState == Enums.GameStateEnum.PreFlop))
                return;
            
            if (GameState != Enums.GameStateEnum.PreFlop)
            {
                foreach (var player in PlayerQueue)
                    player.CurrentBet = 0;
                previousBet = (0, false, false);
            }
            
            var players = PlayerQueue.Where(x => !x.IsAllIn).OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            allInPlayers = PlayerQueue.Where(x => x.IsAllIn).ToList();
            PlayerQueue.Clear();
            for (var i = 1; i < players.Count; i++)
                PlayerQueue.Enqueue(players[i]);
            
            if (players.Count > 0)
            {
                PlayerQueue.Enqueue(players[0]);
                CurrentPlayer = PlayerQueue.Peek();
                foreach (var player in PlayerQueue)
                {
                    player.HasTakenAction = false;
                    player.LastCommand = 0;
                }
            }

            allInPlayers.ForEach(x => x.LastCommand = 0);

            previousBet = (0, false, false);
            currentRaise = (0, 0);
            allInPlayersForRound.Clear();
            maxBetForTurn = 0;
            if (Pots[0].GoesToPlayer != Guid.Empty && GameState != Enums.GameStateEnum.Showdown && GameState != Enums.GameStateEnum.GameOver)
                Pots.Insert(0, new PotEntity(Guid.Empty, 0, 0, null));
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
                PlayerQueue.Enqueue(playerList[i]);
            PlayerQueue.Enqueue(playerList[0]);
            
            CurrentPlayer = PlayerQueue.Peek();
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
            var num = Pots.Count - 2;
            var potNumber = (char)('A' + num);
            while (Pots.Count > 0)
            {
                var firstPot = Pots[0];
                var potPlayer = allPlayerList.FirstOrDefault(x => x.Id == firstPot.GoesToPlayer);
                var eligiblePlayers = firstPot.EligiblePlayers != null
                    ? firstPot.EligiblePlayers
                    : potPlayer == null
                        ? allPlayerList.Where(x => !x.HasFolded && !x.IsAllIn).ToList()
                        : allPlayerList.Where(x => !x.HasFolded && (!x.IsAllIn || showdownDictionary.ContainsKey(x.Id))).Concat([potPlayer]).ToList();
                showdownDictionary.Add(potPlayer == null ? Guid.Empty : potPlayer.Id, eligiblePlayers);
                var winner = GetWinner(showdownDictionary[firstPot.GoesToPlayer]);
                winner.PotName = Pots.Count == 1 ? "Main pot" : $"Side pot {potNumber}";
                winnerList.Add(winner);
                Pots.RemoveAt(0);
                potNumber--;
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
                winningHand.Winner.Chips += Pots[0].PotAmount;
                winningHand.PotToWinner = Pots[0].PotAmount;
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
                var split = Pots[0].PotAmount / 2;
                winningHand.TiedWith.ForEach(player => player.Chips += split);
                winningHand.PotToTiedWith = split;
            }
            else
            {
                winningHand.Winner.Chips += Pots[0].PotAmount;
                winningHand.PotToWinner = Pots[0].PotAmount;
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
    }
}