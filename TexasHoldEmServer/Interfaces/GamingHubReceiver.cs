using System.Text;
using MagicOnion.Server.Hubs;
using THE.GameLogic;
using THE.Managers;
using THE.Shared.Enums;
using THE.Entities;
using THE.Shared.Utilities;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace THE.Interfaces
{
    public class GamingHubReceiver : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        private IGroup? group;
        private PlayerEntity? self;
        private IInMemoryStorage<PlayerEntity>? storage;
        private IRoomManager? roomManager;
        
        public async Task<Enums.JoinRoomResponseTypeEnum> JoinRoomAsync(string userName)
        {
            try
            {
                if (roomManager == null)
                    roomManager = Context.ServiceProvider.GetService<IRoomManager>();

                self = new PlayerEntity(userName, Guid.NewGuid(), Enums.PlayerRoleEnum.None);
                self.Chips = Constants.StartingChips;
                var existingRoom = roomManager.GetNonFullRoomEntity();
                if (existingRoom == null || existingRoom.Closed)
                {
                    if (roomManager.GetRoomCount() >= RoomManager.MaxRoomCount)
                    {
                        return Enums.JoinRoomResponseTypeEnum.AllRoomsFull;
                    }

                    var roomId = Guid.NewGuid();
                    var gameLogicManager = Context.ServiceProvider.GetService<IGameLogicManager>();
                    var jokerManager = Context.ServiceProvider.GetService<IJokerManager>();
                    self.RoomId = roomId;
                    (group, storage) = await Group.AddAsync(roomId.ToString(), self);
                    roomManager.AddRoomAndConnection(roomId, storage, self.Id, ConnectionId, gameLogicManager, jokerManager);
                }
                else
                {
                    (group, storage) = await Group.AddAsync(existingRoom.Id.ToString(), self);
                    roomManager.AddConnection(existingRoom.Id, self.Id, ConnectionId);
                }

                self.RoomId = Guid.Parse(group.GroupName);
                var room = roomManager.GetRoomEntity(self.RoomId);
                Broadcast(group).OnJoinRoom(self, storage.AllValues.Count, room.JokerManager.GetJokerEntities());
            }
            catch (Exception)
            {
                return Enums.JoinRoomResponseTypeEnum.InternalServerError;
            }

            Console.WriteLine($"{userName} joined");
            return Enums.JoinRoomResponseTypeEnum.Success;
        }

        public async Task<PlayerEntity> LeaveRoomAsync()
        {
            if (group == null)
                return null;
            
            Broadcast(group).OnLeaveRoom(self, storage.AllValues.Count - 1);
            await group.RemoveAsync(Context);
            roomManager.RemoveConnection(self.RoomId, self.Id);
            if (storage.AllValues.Count == 0)
                roomManager.ClearRoom(self.RoomId);
            
            Console.WriteLine($"{self.Name} left");
            return self;
        }

        public async Task<PlayerEntity[]> GetAllPlayers()
        {
            if (group == null)
                return null;
            
            Broadcast(group).OnGetAllPlayers(storage.AllValues.ToList());
            return storage.AllValues.ToArray();
        }

        public async Task<Enums.StartResponseTypeEnum> StartGame(Guid playerId, bool isFirstRound)
        {
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            var room = roomManager.GetRoomEntity(currentPlayer.RoomId);
            if (room.GameLogicManager.GetCurrentRound() > Constants.MaxRounds)
                return Enums.StartResponseTypeEnum.AlreadyPlayedMaxRounds;
            
            if (group == null)
                return Enums.StartResponseTypeEnum.GroupDoesNotExist;

            try
            {
                if (currentPlayer.Chips < Constants.MinBet && !isFirstRound)
                {
                    //not enough chips to play
                    Broadcast(group).OnLeaveRoom(self, storage.AllValues.Count);
                    await group.RemoveAsync(Context);
                    roomManager.RemoveConnection(self.RoomId, self.Id);
                    if (storage.AllValues.Count == 0)
                        roomManager.ClearRoom(self.RoomId);

                    return Enums.StartResponseTypeEnum.NotEnoughChips;
                }

                if (currentPlayer != null)
                    currentPlayer.IsReady = true;

                if (!isFirstRound)
                    players = players.Where(player => player.Chips > Constants.MinBet).ToList();

                if (players.Any(player => !player.IsReady))
                    return Enums.StartResponseTypeEnum.AllPlayersNotReady;

                if (players.Count < Constants.MinimumPlayers)
                    return Enums.StartResponseTypeEnum.NotEnoughPlayers;

                players.ForEach(player => player.IsReady = false);

                room.GameLogicManager.SetupGame(players.ToList(), isFirstRound);
                room.GameLogicManager.CreateQueue(players.ToList());

                
                room.CloseRoom();
                var eligiblePlayers = room.Storage.AllValues.Where(player => players.Select(x => x.Id).Contains(player.Id));
                var ids = new List<Guid>();
                foreach (var player in eligiblePlayers)
                    ids.Add(room.GetConnectionId(player.Id));

                BroadcastTo(group, ids.ToArray()).OnGameStart(room.GameLogicManager.GetPlayerQueue().ToList(), room.GameLogicManager.GetCurrentPlayer(), room.GameLogicManager.GetGameState(), room.GameLogicManager.GetCurrentRound(), isFirstRound);
            }
            catch (Exception)
            {
                return Enums.StartResponseTypeEnum.InternalServerError;
            }

            return Enums.StartResponseTypeEnum.Success;
        }

        public async Task CancelStart(Guid playerId)
        {
            if (group == null)
                return;
            
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            if (currentPlayer != null)
                currentPlayer.IsReady = false;
            
            Broadcast(group).OnCancelGameStart();
        }

        public async Task<Enums.DoActionResponseTypeEnum> DoAction(Guid playerId, Enums.CommandTypeEnum commandType, int betAmount)
        {
            if (group == null)
                return Enums.DoActionResponseTypeEnum.GroupDoesNotExist;
            
            if (storage.AllValues.Any(player => !player.CardsAreValid()))
                return Enums.DoActionResponseTypeEnum.PlayerHasInvalidCardData;

            var player = storage.AllValues.First(x => x.Id == playerId);
            var gameLogicManager = roomManager.GetRoomEntity(player.RoomId).GameLogicManager;
            try
            {
                var previousPlayer = gameLogicManager.GetCurrentPlayer();
                gameLogicManager.DoAction(commandType, betAmount, out bool isGameOver, out bool isError, out string actionMessage);
                Console.WriteLine(actionMessage);
                if (isError)
                    BroadcastToSelf(group).OnDoAction(commandType, storage.AllValues.ToList(), previousPlayer.Id, gameLogicManager.GetCurrentPlayer().Id, gameLogicManager.GetPots(), gameLogicManager.GetCommunityCards(), gameLogicManager.GetGameState(), gameLogicManager.GetCurrentExtraBettingRound(), isError, actionMessage, []);
                else if (isGameOver)
                {
                    var winnerList = gameLogicManager.DoShowdown();
                    Broadcast(group).OnDoAction(commandType, storage.AllValues.ToList(), previousPlayer.Id, gameLogicManager.GetCurrentPlayer().Id, gameLogicManager.GetPots(), gameLogicManager.GetCommunityCards(), gameLogicManager.GetGameState(), gameLogicManager.GetCurrentExtraBettingRound(), isError, actionMessage, winnerList);
                }
                else
                {
                    var winnerList = new List<WinningHandEntity>();
                    if (gameLogicManager.GetGameState() == Enums.GameStateEnum.Showdown)
                        winnerList = gameLogicManager.DoShowdown();

                    Broadcast(group).OnDoAction(commandType, storage.AllValues.ToList(), previousPlayer.Id, gameLogicManager.GetCurrentPlayer().Id, gameLogicManager.GetPots(), gameLogicManager.GetCommunityCards(), gameLogicManager.GetGameState(), gameLogicManager.GetCurrentExtraBettingRound(), isError, actionMessage, winnerList);
                }
            }
            catch (Exception ex)
            {
                return Enums.DoActionResponseTypeEnum.InternalServerError;
            }
            
            return Enums.DoActionResponseTypeEnum.Success;
        }

        public async Task<Enums.BuyJokerResponseTypeEnum> BuyJoker(Guid playerId, int jokerId)
        {
            if (group == null)
                return Enums.BuyJokerResponseTypeEnum.GroupDoesNotExist;

            var player = storage.AllValues.First(x => x.Id == playerId);
            var room = roomManager.GetRoomEntity(player.RoomId);
            Enums.BuyJokerResponseTypeEnum response;
            try
            {
                response = room.JokerManager.PurchaseJoker(jokerId, player, out bool isError, out JokerEntity addedJoker);
                if (response == Enums.BuyJokerResponseTypeEnum.Success)
                {
                    Console.WriteLine($"Player {player.Name} is purchasing {addedJoker.JokerType} influence joker {addedJoker.JokerId}, response: {response}");
                    BroadcastToSelf(group).OnBuyJoker(player, addedJoker);
                }
            }
            catch (Exception)
            {
                return Enums.BuyJokerResponseTypeEnum.InternalServerError;
            }

            return response;
        }

        public async Task<Enums.UseJokerResponseTypeEnum> UseJoker(Guid jokerUserId, Guid selectedJokerUniqueId, List<Guid> targetPlayerIds, List<CardEntity> cardEntities)
        {
            if (group == null)
                return Enums.UseJokerResponseTypeEnum.GroupDoesNotExist;
            
            if (storage.AllValues.Any(player => !player.CardsAreValid()))
                return Enums.UseJokerResponseTypeEnum.PlayerHasInvalidCardData;

            var jokerUser = storage.AllValues.First(x => x.Id == jokerUserId);
            var room = roomManager.GetRoomEntity(jokerUser.RoomId);
            var gameLogicManager = room.GameLogicManager;
            var jokerManager = room.JokerManager;
            var jokerEntity = jokerUser.JokerCards.First(x => x.UniqueId == selectedJokerUniqueId);
            Enums.UseJokerResponseTypeEnum response;
            try
            {
                var targetPlayers = new List<PlayerEntity>();
                foreach (var id in targetPlayerIds)
                    targetPlayers.Add(storage.AllValues.First(x => x.Id == id));
                
                
                response = jokerManager.UseJoker(gameLogicManager, jokerUser, targetPlayers, jokerEntity, cardEntities, out bool isError, out bool showHand, out string message);
                if (!isError)
                {
                    var targetNames = targetPlayers.Select(x => x.Name).ToList();
                    var sb = new StringBuilder();
                    targetNames.ForEach(x => sb.Append($"{x} "));
                    Console.WriteLine($"Player {jokerUser.Name} used {jokerEntity.JokerType} influence joker against player(s) {sb}, response: {response}");
                    Broadcast(group).OnUseJoker(storage.AllValues.ToList(), gameLogicManager.GetCommunityCards(), jokerUser, targetPlayers, jokerEntity, gameLogicManager.GetCurrentPlayer().Id, gameLogicManager.GetPots(), isError, showHand, message);
                }
                else
                    BroadcastToSelf(group).OnUseJoker(storage.AllValues.ToList(), gameLogicManager.GetCommunityCards(), jokerUser, targetPlayers, jokerEntity, gameLogicManager.GetCurrentPlayer().Id, gameLogicManager.GetPots(), isError, false, message);
            }
            catch (Exception)
            {
                return Enums.UseJokerResponseTypeEnum.InternalServerError;
            }
            
            return response;
        }

        public async Task DiscardHoleCard(Guid jokerUserId, Guid jokerUniqueId, List<CardEntity> cardsToDiscard)
        {
            var player = storage.AllValues.First(x => x.Id == jokerUserId);
            var joker = player.JokerCards.First(x => x.UniqueId == jokerUniqueId);
            var gameLogicManager = roomManager.GetRoomEntity(player.RoomId).GameLogicManager;
            gameLogicManager.DiscardAndFinishUsingJoker(player, joker, cardsToDiscard);

            var effect = joker.JokerAbilityEntity.AbilityEffects.First();
            var message = $"Player {player.Name} discarded {effect.EffectValue} new card(s).\nプレイヤー{player.Name}が{effect.EffectValue}カードを引いた";
            Broadcast(group).OnDiscardHoleCard(player, cardsToDiscard, message);
        }

        protected override ValueTask OnDisconnected()
        {
            roomManager?.RemoveConnection(self.Id, self.RoomId);
            if (storage.AllValues.Count == 0)
                roomManager.ClearRoom(self.RoomId);
            
            return ValueTask.CompletedTask;
        }
    }
}