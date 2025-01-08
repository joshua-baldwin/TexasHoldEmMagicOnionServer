using THE.Shared.Enums;
using THE.Entities;
using THE.Shared.Utilities;

namespace THE.GameLogic
{
    public class GameLogicManager : IGameLogicManager
    {
        private readonly Queue<PlayerEntity> playerQueue = new();
        //protected for unit testing
        protected List<CardEntity> communityCards = new();
        private readonly List<PlayerEntity> allInPlayersForRound = [];
        private List<PlayerEntity> allPlayerList = [];
        private List<PlayerEntity> allInPlayers = [];
        private bool smallBlindBetDone;
        private bool bigBlindBetDone;
        private (int Amount, bool IsAllIn, bool IsGreaterThanPrevious) previousBet;
        protected List<CardEntity> cardPool = new();
        private bool isTie;
        private int maxBetForTurn;
        private int currentRaise;
        private int chipAmountBeforeAllIn;
        private PlayerEntity previousPlayer;
        private PlayerEntity currentPlayer;
        private List<PotEntity> pots = [new (Guid.Empty, 0, 0, false, null)];
        private Enums.GameStateEnum gameState;
        private int currentExtraBettingRound;
        private int extraBettingRoundCount;
        private int currentRound;

        #region Interface methods

        public List<CardEntity> GetCardPool() => cardPool;
        public Queue<PlayerEntity> GetPlayerQueue() => playerQueue;
        public PlayerEntity GetPreviousPlayer() => previousPlayer;
        public PlayerEntity GetCurrentPlayer() => currentPlayer;
        public List<PotEntity> GetPots() => pots;
        public List<CardEntity> GetCommunityCards() => communityCards;
        public Enums.GameStateEnum GetGameState() => gameState;
        public int GetCurrentRound() => currentRound;
        public int GetCurrentExtraBettingRound() => currentExtraBettingRound;
        public int GetExtraBettingRoundsCount() => extraBettingRoundCount;
        public List<PlayerEntity> GetAllPlayers() => allPlayerList;

        public void Reset()
        {
            allInPlayersForRound?.Clear();
            allPlayerList?.Clear();
            allInPlayers?.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            cardPool?.Clear();
            isTie = false;
            maxBetForTurn = 0;
            currentRaise = 0;
            playerQueue?.Clear();
            previousPlayer = null;
            currentPlayer = null;
            pots = [new PotEntity(Guid.Empty, 0, 0, false, null)];
            communityCards?.Clear();
            gameState = Enums.GameStateEnum.BlindBet;
            currentRound = 0;
            currentExtraBettingRound = 0;
            extraBettingRoundCount = 0;
            chipAmountBeforeAllIn = 0;
        }

        public void SetupGame(List<PlayerEntity> players, bool isFirstRound)
        {
            currentRound++;
            allPlayerList = players;
            cardPool = CreateDeck();
            if (!isFirstRound)
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

            if (!PlayerCanDoAction(commandType, out var message))
            {
                actionMessage = message;
                isError = true;
                return;
            }
            
            switch (commandType)
            {
                case Enums.CommandTypeEnum.SmallBlindBet:
                    var betAmount = Constants.MinBet / 2;
                    if (!CanPlaceBet((betAmount, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{currentPlayer.Name} bet {betAmount}.";
                    previousBet = (betAmount, false, false);
                    currentPlayer.CurrentBet += betAmount;
                    currentPlayer.Chips -= betAmount;
                    currentPlayer.RaiseAmount = betAmount;
                    smallBlindBetDone = true;
                    
                    if (pots.Count == 0 || pots[0].GoesToPlayer != Guid.Empty)
                        pots.Insert(0, new PotEntity(Guid.Empty, betAmount, 0, false, null));
                    else
                    {
                        potEntity = pots[0];
                        potEntity.PotAmount += betAmount;
                        pots[0] = potEntity;
                    }
                    
                    break;
                case Enums.CommandTypeEnum.BigBlindBet:
                    if (!CanPlaceBet((Constants.MinBet, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    actionMessage = $"{currentPlayer.Name} bet {Constants.MinBet}.";
                    previousBet = (Constants.MinBet, false, false);
                    currentPlayer.CurrentBet += Constants.MinBet;
                    currentPlayer.Chips -= Constants.MinBet;
                    currentPlayer.RaiseAmount = Constants.MinBet;
                    bigBlindBetDone = true;
                    maxBetForTurn = Constants.MinBet;
                    
                    potEntity = pots[0];
                    potEntity.PotAmount += Constants.MinBet;
                    pots[0] = potEntity;
                    
                    break;
                case Enums.CommandTypeEnum.Check:
                    if (!CanCheck(out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    
                    currentPlayer.HasTakenAction = true;
                    break;
                case Enums.CommandTypeEnum.Fold:
                    actionMessage = $"{currentPlayer.Name} folded.";
                    currentPlayer.HasFolded = true;
                    RemoveFromPots();
                    if (playerQueue.Count(x => x.HasFolded) == playerQueue.Count + allInPlayers.Count - 1)
                    {
                        isGameOver = true;
                        gameState = Enums.GameStateEnum.GameOver;
                    }
                    break;
                case Enums.CommandTypeEnum.Call:
                    var callAmount = maxBetForTurn - currentPlayer.CurrentBet;
                    if (!CanPlaceBet((callAmount, 0), out message))
                    {
                        actionMessage = message;
                        isError = true;
                        return;
                    }
                    //if there was a call after an all in, add it to the amount so we recalculate pots correctly
                    if (chipAmountBeforeAllIn != 0)
                        chipAmountBeforeAllIn += callAmount;
                    actionMessage = $"{currentPlayer.Name} called.";
                    previousBet = (callAmount, false, false);
                    currentPlayer.CurrentBet += callAmount;
                    currentPlayer.Chips -= callAmount;
                    currentPlayer.HasTakenAction = true;

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
                    //if there was a bet after an all in, add it to the amount so we recalculate pots correctly
                    if (chipAmountBeforeAllIn != 0)
                        chipAmountBeforeAllIn += betAmount;
                    actionMessage = $"{currentPlayer.Name} raised {chipsBet}.";
                    previousBet = (betAmount, false, betAmount > currentRaise);
                    currentPlayer.CurrentBet += betAmount;
                    currentPlayer.Chips -= betAmount;
                    currentPlayer.RaiseAmount = chipsBet;
                    if (betAmount > currentRaise)
                    {
                        foreach (var player in playerQueue.Where(x => !x.IsAllIn))
                        {
                            player.HasTakenAction = false;
                            player.HasHighestRaise = player.Id == currentPlayer.Id;
                        }
                    }
                    
                    currentPlayer.HasTakenAction = true;
                    currentRaise = betAmount;
                    maxBetForTurn += chipsBet;
                    
                    DistributeBetAmountToPots(betAmount);
                    break;
                case Enums.CommandTypeEnum.AllIn:
                    if (allInPlayersForRound.Count == 0)
                        chipAmountBeforeAllIn = pots[0].PotAmount;

                    allInPlayersForRound.Add(currentPlayer);
                    actionMessage = $"{currentPlayer.Name} went all in.";
                    
                    var isRaise = previousBet.Amount != 0 &&
                                  currentPlayer.Chips > previousBet.Amount &&
                                  currentPlayer.Chips > currentRaise;

                    currentPlayer.HasTakenAction = true;
                    currentPlayer.IsAllIn = true;
                    if (isRaise)
                    {
                        currentRaise = currentPlayer.Chips;
                        currentPlayer.HasHighestRaise = true;
                        foreach (var player in playerQueue.Where(x => !x.IsAllIn))
                        {
                            player.HasTakenAction = false;
                            player.HasHighestRaise = false;
                        }
                    }
                    else if (currentPlayer.Chips > previousBet.Amount && currentPlayer.Chips > currentRaise)
                        currentRaise = currentPlayer.Chips;

                    previousBet = (currentPlayer.Chips, true, isRaise);
                    
                    currentPlayer.AllInAmount = currentPlayer.Chips;
                    currentPlayer.CurrentBet += currentPlayer.Chips;
                    currentPlayer.Chips = 0;
                    if (currentPlayer.AllInAmount > maxBetForTurn)
                        maxBetForTurn = currentPlayer.AllInAmount;

                    RecalculatePots();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }

            currentPlayer.ActiveEffects.RemoveAll(x => x.ActionInfluenceType is Enums.ActionInfluenceTypeEnum.Force or Enums.ActionInfluenceTypeEnum.Prevent);

            previousPlayer = playerQueue.Dequeue();
            previousPlayer.LastCommand = commandType;
            if (!currentPlayer.HasFolded && (currentPlayer.Chips > 0 || currentPlayer.Chips == 0 && currentPlayer.IsAllIn))
                playerQueue.Enqueue(currentPlayer);
            currentPlayer = playerQueue.Peek();
            if (playerQueue.Any(x => !x.IsAllIn))
            {
                while (currentPlayer.IsAllIn)
                {
                    currentPlayer = playerQueue.Dequeue();
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Peek();
                }
            }

            UpdateGameState();
        }

        public void DiscardAndFinishUsingJoker(PlayerEntity target, JokerEntity joker, List<CardEntity> cardsToDiscard)
        {
            DiscardToCardPool(target, cardsToDiscard);
            if (target.TempHoleCards.Count > 0)
            {
                target.HoleCards.AddRange(target.TempHoleCards);
                target.TempHoleCards.Clear();
                target.MaxHoleCards = target.HoleCards.Count;
            }
            
            joker.CurrentUses++;
            if (joker.CurrentUses >= joker.MaxUses)
                target.JokerCards.RemoveAll(x => x.UniqueId == joker.UniqueId);
        }

        public void DiscardToCardPool(PlayerEntity target, List<CardEntity> cardsToDiscard)
        {
            foreach (var card in cardsToDiscard)
            {
                var playerCard = target.HoleCards.FirstOrDefault(x => x.Rank == card.Rank && x.Suit == card.Suit);
                if (playerCard == null)
                {
                    playerCard = target.TempHoleCards.First(x => x.Rank == card.Rank && x.Suit == card.Suit);
                    target.TempHoleCards.Remove(playerCard);    
                }
                else
                    target.HoleCards.Remove(playerCard);

                cardPool.Add(playerCard);
            }
        }

        public List<CardEntity> DrawFromCardPool(int numberOfCardsToDraw)
        {
            var newCards = new List<CardEntity>();
            for (var i = 0; i < numberOfCardsToDraw; i++)
            {
                var card = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
                cardPool.Remove(card);
                newCards.Add(card);
            }
            return newCards;
        }
        
        public void CreateQueue(List<PlayerEntity> players)
        {
            var playerList = players.OrderByDescending(x => x.IsDealer)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.SmallBlind)
                .ThenByDescending(x => x.PlayerRole == Enums.PlayerRoleEnum.BigBlind).ToList();
            for (var i = 1; i < playerList.Count; i++)
                playerQueue.Enqueue(playerList[i]);
            playerQueue.Enqueue(playerList[0]);

            var order = 1;
            foreach (var player in playerQueue)
            {
                player.OrderInQueue = order;
                player.OriginalOrderInQueue = order;
                order++;
            }
            
            currentPlayer = playerQueue.Peek();
        }
        
        public List<WinningHandEntity> DoShowdown()
        {
            var winnerList = new List<WinningHandEntity>();
            var showdownDictionary = new Dictionary<Guid, List<PlayerEntity>>();
            var num = pots.Count - 2;
            var potNumber = (char)('A' + num);
            while (pots.Count > 0)
            {
                var firstPot = pots[0];
                var potPlayer = allPlayerList.FirstOrDefault(x => x.Id == firstPot.GoesToPlayer);
                var eligiblePlayers = firstPot.EligiblePlayers != null
                    ? firstPot.EligiblePlayers
                    : potPlayer == null
                        ? allPlayerList.Where(x => !x.HasFolded && !x.IsAllIn).ToList()
                        : allPlayerList.Where(x => !x.HasFolded && (!x.IsAllIn || showdownDictionary.ContainsKey(x.Id))).Concat([potPlayer]).ToList();
                showdownDictionary.Add(potPlayer == null ? Guid.Empty : potPlayer.Id, eligiblePlayers);
                var winner = GetWinner(showdownDictionary[firstPot.GoesToPlayer]);
                winner.PotName = pots.Count == 1 ? "Main pot" : $"Side pot {potNumber}";
                winnerList.Add(winner);
                pots.RemoveAt(0);
                potNumber--;
            }
            
            return winnerList;
        }

        public void AddJokerCostToPot(int cost)
        {
            var potEntity = pots[0];
            potEntity.PotAmount += cost;
            pots[0] = potEntity;
            if (chipAmountBeforeAllIn != 0)
                chipAmountBeforeAllIn += cost;
        }

        public void UpdateQueue(PlayerEntity playerToChange)
        {
            var activePlayers = playerQueue.ToList();
            var numberOfPlayersToChange = playerQueue.Count - playerToChange.OrderInQueue + 1;
            for (var i = 0; i < numberOfPlayersToChange; i++)
            {
                if (activePlayers[i].Id == playerToChange.Id)
                    playerToChange.OrderInQueue = playerQueue.Count;
                else
                    activePlayers[i].OrderInQueue -= 1;
            }
            
            var players = activePlayers.OrderBy(x => x.OrderInQueue).ToList();
            playerQueue.Clear();
            foreach (var player in players)
                playerQueue.Enqueue(player);

            while (playerQueue.Peek().HasTakenAction)
            {
                var player = playerQueue.Dequeue();
                playerQueue.Enqueue(player);
            }
            
            currentPlayer = playerQueue.Peek();
        }

        public void IncreaseNumberOfBettingRounds()
        {
            extraBettingRoundCount++;
        }

        public void UpdateCardWeight(List<CardEntity> cards, int multiplier, bool increaseWeight)
        {
            foreach (var card in cards)
            {
                var oldWeight = cardPool.First(x => x.Suit == card.Suit && x.Rank == card.Rank).Weight;
                cardPool.First(x => x.Suit == card.Suit && x.Rank == card.Rank).Weight = increaseWeight
                    ? oldWeight * multiplier
                    : oldWeight / multiplier;
            }
        }

        #endregion

        private void RemoveFromPots()
        {
            pots.ForEach(pot =>
            {
                pot.EligiblePlayers?.RemoveAll(x => x.Id == currentPlayer.Id);
            });
        }

        private void DistributeBetAmountToPots(int callAmount)
        {
            PotEntity potEntity;
            var potIndex = pots.Count - 1;
            while (callAmount > 0 && potIndex >= 0 && pots[potIndex].GoesToPlayer != Guid.Empty && !pots[potIndex].IsLocked)
            {
                var allInAmount = playerQueue.FirstOrDefault(x => x.Id == pots[potIndex].GoesToPlayer)?.CurrentBet;
                potEntity = pots[potIndex];
                if (allInAmount.HasValue && allInAmount.Value != 0)
                {
                    var toPot = callAmount - allInAmount.Value > 0 ? allInAmount.Value : callAmount;
                    potEntity.PotAmount += toPot;
                    callAmount -= toPot;
                }
                else
                {
                    potEntity.PotAmount += callAmount;
                    callAmount -= callAmount;
                }

                pots[potIndex] = potEntity;
                potIndex--;
            }

            if (callAmount <= 0)
                return;
            
            if (pots[0].GoesToPlayer != Guid.Empty)
                pots.Insert(0, new PotEntity(Guid.Empty, callAmount, 0, false, null));
            else
            {
                potEntity = pots[0];
                potEntity.PotAmount += callAmount;
                pots[0] = potEntity;
            }
        }
        
        private void RecalculatePots()
        {
            PotEntity potEntity;
            //if another person goes all in for a lower amount need to create a new pot and insert to the bottom and recalculate pot amounts
            var createNewPot = pots.Any(x => x.AllInAmount != 0 && x.AllInAmount > currentPlayer.AllInAmount && !x.IsLocked);
            if (pots.Count == 0 || createNewPot)
            {
                var index = pots.IndexOf(pots.Last(x => x.AllInAmount > currentPlayer.AllInAmount && !x.IsLocked));
                var eligiblePlayers = playerQueue.Where(x => !x.IsAllIn).Concat(allInPlayersForRound.Where(x => x.CurrentBet >= currentPlayer.AllInAmount)).ToList();
                //if multiple players go all in on the same turn and multiple pots are created, set the amount of the new pot to the amount that was in the main pot before anyone went all in
                //set the pot amount of the other pots to 0 since we recalculate in the while loop
                pots.ForEach(pot =>
                {
                    if (!pot.IsLocked)
                        pot.PotAmount = 0;
                });
                pots.Insert(index + 1, new PotEntity(currentPlayer.Id, chipAmountBeforeAllIn, currentPlayer.AllInAmount, currentPlayer.AllInAmount <= previousBet.Amount, eligiblePlayers));
                var amounts = playerQueue.Where(x => x.IsAllIn).OrderByDescending(x => x.AllInAmount).Select(x => x.AllInAmount).ToList();
                var potIndex = pots.Count(x => !x.IsLocked) - 1;
                while (amounts.Count > 0)
                {
                    var toTake = amounts.Last() * amounts.Count;
                    potEntity = pots[potIndex];
                    potEntity.PotAmount += toTake;
                    pots[potIndex] = potEntity;
                    potIndex--;
                            
                    for (var i = 0; i < amounts.Count; i++)
                        amounts[i] -= amounts.Last();

                    amounts.RemoveAll(x => x == 0);
                }
            }
            else
            {
                var allInBet = currentPlayer.AllInAmount;
                potEntity = pots[0];
                potEntity.GoesToPlayer = currentPlayer.Id;
                potEntity.PotAmount += allInBet;
                potEntity.AllInAmount = currentPlayer.AllInAmount;
                potEntity.EligiblePlayers = playerQueue.Where(x => !x.IsAllIn).Concat(allInPlayersForRound.Where(x => x.CurrentBet >= currentPlayer.AllInAmount)).ToList();
                pots[0] = potEntity;
                        
                var sidePot = 0;
                foreach (var player in playerQueue.Where(x => x.Id != currentPlayer.Id))
                {
                    if (allInBet >= player.CurrentBet)
                        continue;
                            
                    var extra = player.CurrentBet - allInBet;
                    potEntity = pots[0];
                    potEntity.PotAmount -= extra;
                    pots[0] = potEntity;
                    sidePot += extra;
                }

                if (sidePot != 0)
                {
                    if (pots[0].GoesToPlayer != Guid.Empty)
                        pots.Insert(0, new PotEntity(Guid.Empty, sidePot, 0, false, playerQueue.Where(x => !x.IsAllIn && !x.HasFolded).ToList()));
                    else
                    {
                        potEntity = pots[0];
                        potEntity.PotAmount += sidePot;
                        pots[0] = potEntity;
                    }   
                }
            }

            //if another player goes all in, if they were placed in the eligible list beforehand and their all in is a lower amount, remove them from the list
            foreach (var pot in pots)
            {
                if (pot.GoesToPlayer == Guid.Empty)
                    pot.EligiblePlayers?.RemoveAll(x => x.IsAllIn);
                else
                {
                    var allInPlayer = playerQueue.FirstOrDefault(x => x.Id == pot.GoesToPlayer);
                    var self = pot.EligiblePlayers?.FirstOrDefault(x => x.Id == currentPlayer.Id);
                    if (self?.CurrentBet < allInPlayer?.CurrentBet)
                        pot.EligiblePlayers.RemoveAll(x => x.Id == self.Id);
                }
            }
        }

        private void InitializeForNextRound()
        {
            allInPlayersForRound?.Clear();
            allInPlayers?.Clear();
            smallBlindBetDone = false;
            bigBlindBetDone = false;
            previousBet = (0, false, false);
            isTie = false;
            maxBetForTurn = 0;
            currentRaise = 0;
            playerQueue.Clear();
            previousPlayer = null;
            currentPlayer = null;
            pots = [new PotEntity(Guid.Empty, 0, 0, false, null)];
            communityCards.Clear();
            gameState = Enums.GameStateEnum.BlindBet;
            chipAmountBeforeAllIn = 0;
        }

        private void UpdateGameState()
        {
            var gameStateChanged = false;
            if (playerQueue.Count == 1 || playerQueue.All(x => x.IsAllIn) || (playerQueue.Count(x => x.IsAllIn) == playerQueue.Count - 1 && playerQueue.All(x => x.HasTakenAction)))
            {
                if (gameState == Enums.GameStateEnum.GameOver)
                    gameStateChanged = true;
                else
                {
                    gameState = Enums.GameStateEnum.Showdown;
                    SetRemainingCards();
                    gameStateChanged = true;
                }
            }
            else
            {
                switch (gameState)
                {
                    case Enums.GameStateEnum.BlindBet:
                        if (smallBlindBetDone && bigBlindBetDone)
                        {
                            gameState = Enums.GameStateEnum.PreFlop;
                            SetCards(playerQueue.ToList());
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.PreFlop:
                        if (playerQueue.All(x => x.HasTakenAction))
                        {
                            gameState = Enums.GameStateEnum.TheFlop;
                            SetTheFlop();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheFlop:
                        if (playerQueue.All(x => x.HasTakenAction))
                        {
                            gameState = Enums.GameStateEnum.TheTurn;
                            SetTheTurn();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheTurn:
                        if (playerQueue.All(x => x.HasTakenAction))
                        {
                            gameState = Enums.GameStateEnum.TheRiver;
                            SetTheRiver();
                            gameStateChanged = true;
                        }

                        break;
                    case Enums.GameStateEnum.TheRiver:
                        if (playerQueue.All(x => x.HasTakenAction))
                        {
                            if (extraBettingRoundCount > 0)
                            {
                                if (currentExtraBettingRound >= extraBettingRoundCount)
                                {
                                    currentExtraBettingRound = 0;
                                    gameState = Enums.GameStateEnum.Showdown;
                                }
                                else
                                    currentExtraBettingRound++;
                            }
                            else
                                gameState = Enums.GameStateEnum.Showdown;
                            gameStateChanged = true;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!gameStateChanged || (gameStateChanged && gameState == Enums.GameStateEnum.PreFlop))
                return;
            
            if (gameState != Enums.GameStateEnum.PreFlop)
            {
                foreach (var player in playerQueue)
                    player.CurrentBet = 0;
                previousBet = (0, false, false);
            }

            foreach (var player in playerQueue)
                player.OrderInQueue = player.OriginalOrderInQueue;

            var players = playerQueue.Where(x => !x.IsAllIn).OrderBy(x => x.OrderInQueue).ToList();
            allInPlayers = playerQueue.Where(x => x.IsAllIn).ToList();
            playerQueue.Clear();
            foreach (var player in players)
                playerQueue.Enqueue(player);

            if (players.Count > 0)
            {
                currentPlayer = playerQueue.Peek();
                foreach (var player in playerQueue)
                {
                    player.HasTakenAction = false;
                    player.LastCommand = 0;
                    player.HasHighestRaise = false;
                }
            }

            allInPlayers.ForEach(x => x.LastCommand = 0);

            previousBet = (0, false, false);
            currentRaise = 0;
            allInPlayersForRound.Clear();
            maxBetForTurn = 0;
            chipAmountBeforeAllIn = 0;
            if (pots[0].GoesToPlayer != Guid.Empty && gameState != Enums.GameStateEnum.Showdown && gameState != Enums.GameStateEnum.GameOver)
                pots.Insert(0, new PotEntity(Guid.Empty, 0, 0, false, null));

            for (var i = 0; i < pots.Count; i++)
            {
                var potEntity = pots[i];
                if (potEntity.GoesToPlayer != Guid.Empty)
                    potEntity.IsLocked = true;
                pots[i] = potEntity;
            }

            foreach (var player in playerQueue)
            {
                for (var i = player.ActiveEffects.Count - 1; i >= 0; i--)
                {
                    if (player.ActiveEffects[i].ActionInfluenceType == Enums.ActionInfluenceTypeEnum.IncreaseBettingRounds)
                    {
                        i--;
                        continue;
                    }

                    player.ActiveEffects.RemoveAt(i);
                    break;
                }
            }
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
                //if dealer dropped out of game get a new random
                var dealer = players.FirstOrDefault(x => x.IsDealer);
                if (dealer != null)
                    dealer.IsDealer = false;
                var dealerIndex = dealer == null ? -1 : players.IndexOf(dealer);
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
                for (var i = 0; i < Constants.MaxHoleCards; i++)
                {
                    var card = WeightedCardSelector.GetCardEntity(shuffled, shuffled.Sum(x => x.Weight));
                    shuffled.Remove(card);
                    players[startIndex].HoleCards.Add(card);
                    players[startIndex].MaxHoleCards++;
                }
                
                dealt++;
                if (startIndex + 1 >= players.Count)
                    startIndex = 0;
                else
                    startIndex++;
            }

            cardPool = shuffled;
        }
        
        private List<CardEntity> CreateDeck()
        {
            var suits = Enum.GetValues<Enums.CardSuitEnum>();
            var ranks = Enum.GetValues<Enums.CardRankEnum>();
            var deck = new List<CardEntity>();
            foreach (var suit in suits.Where(x => x != Enums.CardSuitEnum.None))
            {
                deck.AddRange(ranks.Select(rank => new CardEntity(suit, rank)));
            }

            return deck;
        }
        
        #endregion
        
        #region Setting community cards
        
        private void SetTheFlop()
        {
            var card1 = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
            cardPool.Remove(card1);
            var card2 = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
            cardPool.Remove(card2);
            var card3 = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
            cardPool.Remove(card3);
            
            communityCards.Add(card1);
            communityCards.Add(card2);
            communityCards.Add(card3);
        }
        
        private void SetTheTurn()
        {
            var card4 = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
            cardPool.Remove(card4);
            communityCards.Add(card4);
        }

        private void SetTheRiver()
        {
            var card5 = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
            cardPool.Remove(card5);
            communityCards.Add(card5);
        }

        private void SetRemainingCards()
        {
            while (communityCards.Count < 5)
            {
                var card = WeightedCardSelector.GetCardEntity(cardPool, cardPool.Sum(x => x.Weight));
                cardPool.Remove(card);
                communityCards.Add(card);
            }
        }
        
        #endregion
        
        private WinningHandEntity GetWinner(List<PlayerEntity> showdownPlayers)
        {
            var winningHand = new WinningHandEntity();
            if (showdownPlayers.Count == 1)
            {
                var player = showdownPlayers.First();
                if (gameState != Enums.GameStateEnum.GameOver)
                {
                    var hand = player.HoleCards.Concat(communityCards).ToList();
                    var rank = HandRankingLogic.GetHandRanking(hand);
                    player.BestHand = new BestHandEntity { Cards = hand.Where(x => x.IsFinalHand).ToList(), HandRanking = rank };
                    winningHand.HandRanking = rank;
                    winningHand.Cards = player.BestHand.Cards;
                }

                winningHand.Winner = player;
                winningHand.Winner.Chips += pots[0].PotAmount;
                winningHand.PotToWinner = pots[0].PotAmount;
                return winningHand;
            }
            
            foreach (var player in showdownPlayers)
            {
                Enums.HandRankingType ranking;
                if (player.BestHand == null)
                {
                    var hand = player.HoleCards.Concat(communityCards).ToList();
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
                    var winnerToCompare = winningHand.Winner ?? winningHand.TiedWith.First();
                    var guid = HandRankingLogic.CompareHands((winnerToCompare.Id, winningHand.Cards), (player.Id, player.BestHand.Cards), ranking);
                    if (guid == Guid.Empty)
                    {
                        isTie = true;
                        if (winningHand.TiedWith.All(x => x.Id != winnerToCompare.Id))
                            winningHand.TiedWith.Add(winnerToCompare);
                        if (winningHand.TiedWith.All(x => x.Id != player.Id))
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
                var split = pots[0].PotAmount / 2;
                winningHand.TiedWith.ForEach(player => player.Chips += split);
                winningHand.PotToTiedWith = split;
            }
            else
            {
                winningHand.Winner.Chips += pots[0].PotAmount;
                winningHand.PotToWinner = pots[0].PotAmount;
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
            
            //if bet equals chips make player go all in
            if (chipsBet.BetAmount >= currentPlayer.Chips)
            {
                message = "Not enough chips.\nチップが足りない。";
                return false;
            }

            message = "";
            return true;
        }

        private bool CanCheck(out string message)
        {
            var anyPlayerHasRaised = playerQueue.Any(x => x.HasTakenAction &&
                                                          x.LastCommand == Enums.CommandTypeEnum.Raise ||
                                                          (x.LastCommand == Enums.CommandTypeEnum.AllIn && x.HasHighestRaise));

            if (gameState == Enums.GameStateEnum.PreFlop)
            {
                if (currentPlayer.PlayerRole != Enums.PlayerRoleEnum.BigBlind)
                {
                    message = "Only the big blind can check during pre-flop\nPre-Flopの時にチェックできるのはビッグブラインドのみ。";
                    return false;
                }

                if (anyPlayerHasRaised)
                {
                    message = "You can't check because a player has raised.\n他のプレイヤーがレイズしたのでチェックできない。";
                    return false;
                }
            }
            else
            {
                if (anyPlayerHasRaised)
                {
                    message = "You can't check because a player has raised.\n他のプレイヤーがレイズしたのでチェックできない。";
                    return false;
                }
            }
            
            message = "";
            return true;
        }

        private bool PlayerCanDoAction(Enums.CommandTypeEnum command, out string message)
        {
            foreach (var effect in currentPlayer.ActiveEffects)
            {
                if (effect.ActionInfluenceType == Enums.ActionInfluenceTypeEnum.Prevent && effect.CommandType == command)
                {
                    message = $"You can't {command} because of the Joker's effects.\nジョーカーの効果で{command}はできない。";
                    return false;    
                }

                if (effect.ActionInfluenceType == Enums.ActionInfluenceTypeEnum.Force && effect.CommandType != command)
                {
                    message = $"You must {effect.CommandType} because of the Joker's effects.\nジョーカーの効果で{effect.CommandType}をしないといけない。";
                    return false;
                }
            }
            
            message = "";
            return true;
        }
    }
}