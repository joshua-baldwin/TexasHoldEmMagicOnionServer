using MagicOnion.Server.Hubs;
using TexasHoldEmServer.GameLogic;
using TexasHoldEmServer.Managers;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Interfaces;

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
        
        public async Task<bool> JoinRoomAsync(string userName)
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
                    return false;
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
            
            Console.WriteLine($"{userName} joined");
            return true;
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

        public async Task<bool> StartGame(Guid playerId)
        {
            if (group == null)
                return false;
            
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            if (currentPlayer != null)
                currentPlayer.IsReady = true;
            
            if (players.Any(player => !player.IsReady))
                return false;
            
            gameLogicManager.SetupGame(ref players);

            Broadcast(group).OnGameStart(players.ToArray(), gameLogicManager.CurrentPlayer, gameLogicManager.GameState);
            return true;
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
            if (isError)
                BroadcastTo(group, ConnectionId).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pot, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, [], Enums.HandRankingType.Nothing);
            else if (isGameOver)
                Broadcast(group).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pot, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, [storage.AllValues.First(x => !x.HasFolded).Id], Enums.HandRankingType.Nothing);
            else
            {
                var winnerIds = new List<Guid>();
                var winningHand = Enums.HandRankingType.Nothing;
                if (gameLogicManager.GameState == Enums.GameStateEnum.Showdown)
                    (winnerIds, winningHand) = gameLogicManager.GetWinner();
                
                Broadcast(group).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, targetPlayerId, gameLogicManager.Pot, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage, winnerIds, winningHand);
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