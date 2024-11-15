using MagicOnion.Server.Hubs;
using TexasHoldEmServer.GameLogic;
using TexasHoldEmServer.ServerManager;
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
        private IGroup? room;
        private PlayerEntity? self;
        private IInMemoryStorage<PlayerEntity>? storage;
        private IServerManager? serverManager;
        private GameLogicManager? gameLogicManager;
        
        public async Task<PlayerEntity> JoinRoomAsync(string userName)
        {
            if (serverManager == null)
                serverManager = Context.ServiceProvider.GetService<IServerManager>();
            
            if (gameLogicManager == null)
                gameLogicManager = Context.ServiceProvider.GetService<GameLogicManager>();

            self = new PlayerEntity(userName, Guid.NewGuid(), Enums.PlayerRoleEnum.None);
            var group = serverManager.GetNonFullRoomEntity();
            if (group == null)
            {
                var roomId = Guid.NewGuid();
                self.RoomId = roomId;
                (room, storage) = await Group.AddAsync(roomId.ToString(), self);
                serverManager.AddGroup(roomId, new RoomEntity(roomId, room, storage));
            }
            else
            {
                (room, storage) = await Group.AddAsync(group.Id.ToString(), self);
            }

            self.RoomId = Guid.Parse(room.GroupName);
            Broadcast(room).OnJoinRoom(self, storage.AllValues.Count);
            
            Console.WriteLine($"{userName} joined");
            return self;
        }

        public async Task<PlayerEntity> LeaveRoomAsync()
        {
            if (room == null)
                return null;
            
            await room.RemoveAsync(Context);
            Broadcast(room).OnLeaveRoom(self, storage.AllValues.Count);
            
            Console.WriteLine($"{self.Name} left");
            return self;
        }

        public async Task<PlayerEntity[]> GetAllPlayers()
        {
            if (room == null)
                return null;
            
            Broadcast(room).OnGetAllPlayers(storage.AllValues.ToArray());
            return storage.AllValues.ToArray();
        }

        public async Task<bool> StartGame(Guid playerId)
        {
            if (room == null)
                return false;
            
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            if (currentPlayer != null)
                currentPlayer.IsReady = true;
            
            if (players.Any(player => !player.IsReady))
                return false;
            
            gameLogicManager.SetupGame(players);

            Broadcast(room).OnGameStart(storage.AllValues.ToArray(), gameLogicManager.CurrentPlayer, gameLogicManager.GameState);
            return true;
        }

        public async Task CancelStart(Guid playerId)
        {
            if (room == null)
                return;
            
            var players = storage.AllValues.ToList();
            var currentPlayer = players.FirstOrDefault(player => player.Id == playerId);
            if (currentPlayer != null)
                currentPlayer.IsReady = false;
            
            Broadcast(room).OnCancelGameStart();
        }

        public async Task QuitGame(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public async Task DoAction(Enums.CommandTypeEnum commandType, int betAmount)
        {
            if (room == null)
                return;

            var previousPlayer = gameLogicManager.CurrentPlayer;
            gameLogicManager.DoAction(commandType, betAmount, out bool isError, out string actionMessage);
            Broadcast(room).OnDoAction(commandType, storage.AllValues.ToArray(), previousPlayer.Id, gameLogicManager.CurrentPlayer.Id, gameLogicManager.Pot, gameLogicManager.CommunityCards, gameLogicManager.GameState, isError, actionMessage);
        }

        protected override ValueTask OnDisconnected()
        {
            return ValueTask.CompletedTask;
        }
    }
}