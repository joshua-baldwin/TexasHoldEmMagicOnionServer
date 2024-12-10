using MagicOnion.Server.Hubs;
using TexasHoldEmServer.GameLogic;
using TexasHoldEmServer.Managers;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Interfaces;
using THE.MagicOnion.Shared.Utilities;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace TexasHoldEmServer.Interfaces
{
    public class GamingHubReceiver : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        private IGroup? group;
        private PlayerEntity? self;
        private IInMemoryStorage<PlayerEntity>? storage;
        private IRoomManager? roomManager;
        private GameLogicManager? gameLogicManager;
        
        public async Task<Enums.JoinRoomResponseTypeEnum> JoinRoomAsync(string userName)
        {
            try
            {
                if (roomManager == null)
                    roomManager = Context.ServiceProvider.GetService<IRoomManager>();

                if (gameLogicManager == null)
                    gameLogicManager = Context.ServiceProvider.GetService<GameLogicManager>();

                self = new PlayerEntity(userName, Guid.NewGuid(), Enums.PlayerRoleEnum.None);
                var existingRoom = roomManager.GetNonFullRoomEntity();
                if (existingRoom == null)
                {
                    if (roomManager.GetRoomCount() >= RoomManager.MaxRoomCount)
                    {
                        return Enums.JoinRoomResponseTypeEnum.AllRoomsFull;
                    }

                    var roomId = Guid.NewGuid();
                    self.RoomId = roomId;
                    (group, storage) = await Group.AddAsync(roomId.ToString(), self);
                    roomManager.AddRoomAndConnection(roomId, group, storage, self.Id, ConnectionId);
                }
                else
                {
                    (group, storage) = await Group.AddAsync(existingRoom.Id.ToString(), self);
                    roomManager.AddConnection(existingRoom.Id, self.Id, ConnectionId);
                }

                self.RoomId = Guid.Parse(group.GroupName);
                Broadcast(group).OnJoinRoom(self, storage.AllValues.Count);
            }
            catch (Exception)
            {
                return Enums.JoinRoomResponseTypeEnum.Failed;
            }

            Console.WriteLine($"{userName} joined");
            return Enums.JoinRoomResponseTypeEnum.Success;
        }

        public async Task<PlayerEntity> LeaveRoomAsync()
        {
            if (group == null)
                return null;
            
            Broadcast(group).OnLeaveRoom(self, storage.AllValues.Count);
            await group.RemoveAsync(Context);
            roomManager.RemoveConnection(self.RoomId, self.Id);
            if (storage.AllValues.Count == 0)
            {
                roomManager.ClearRooms();
                gameLogicManager.Reset();
            }
            
            Console.WriteLine($"{self.Name} left");
            return self;
        }

        public async Task<PlayerEntity[]> GetAllPlayers()
        {
            if (group == null)
                return null;
            
            Broadcast(group).OnGetAllPlayers(storage.AllValues.ToArray());
            return storage.AllValues.ToArray();
        }

        public async Task<Enums.StartResponseTypeEnum> StartGame(Guid playerId, bool isFirstRound)
        {
            if (gameLogicManager.CurrentRound > Constants.MaxRounds)
                return Enums.StartResponseTypeEnum.AlreadyPlayedMaxRounds;
            
            if (group == null)
                return Enums.StartResponseTypeEnum.GroupDoesNotExist;
            
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            if (currentPlayer.Chips < Constants.MinBet && !isFirstRound)
            {
                //not enough chips to play
                Broadcast(group).OnLeaveRoom(self, storage.AllValues.Count);
                await group.RemoveAsync(Context);
                roomManager.RemoveConnection(self.RoomId, self.Id);
                if (storage.AllValues.Count == 0)
                {
                    roomManager.ClearRooms();
                    gameLogicManager.Reset();
                }
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
            
            gameLogicManager.SetupGame(players.ToList(), isFirstRound);
            gameLogicManager.CreateQueue(players.ToList());

            Broadcast(group).OnGameStart(gameLogicManager.PlayerQueue.ToArray(), gameLogicManager.CurrentPlayer, gameLogicManager.GameState, gameLogicManager.CurrentRound, isFirstRound);
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

        public async Task DoAction(Enums.CommandTypeEnum commandType, int betAmount, Guid targetPlayerId)
        {
            if (group == null)
                return;
    
            var previousPlayer = gameLogicManager.CurrentPlayer;
            gameLogicManager.DoAction(commandType, betAmount, out bool isGameOver, out bool isError, out string actionMessage);
            Console.WriteLine(actionMessage);
            if (isError)
                BroadcastTo(group, ConnectionId).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pots, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, []);
            else if (isGameOver)
            {
                var winnerList = gameLogicManager.DoShowdown();
                Broadcast(group).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pots, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, winnerList);
            }
            else
            {
                var winnerList = new List<WinningHandEntity>();
                if (gameLogicManager.GameState == Enums.GameStateEnum.Showdown)
                    winnerList = gameLogicManager.DoShowdown();
                
                Broadcast(group).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pots, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, winnerList);
            }
        }

        protected override ValueTask OnDisconnected()
        {
            roomManager?.RemoveConnection(self.Id, self.RoomId);
            if (storage.AllValues.Count == 0)
            {
                roomManager.ClearRooms();
                gameLogicManager.Reset();
            }
            return ValueTask.CompletedTask;
        }
    }
}