using MagicOnion.Server.Hubs;
using TexasHoldEmServer.ServerManager;
using TexasHoldEmShared.Enums;
using THE.MagicOnion.Shared.Entities;
using THE.MagicOnion.Shared.Interfaces;
using THE.Utilities;

namespace TexasHoldEmServer.Interfaces
{
    public class GamingHubReceiver : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        private IGroup? room;
        private PlayerEntity? self;
        private IInMemoryStorage<PlayerEntity>? storage;
        private IServerManager? serverManager;
        
        public async Task<PlayerEntity> JoinRoomAsync(string userName)
        {
            if (serverManager == null)
                serverManager = Context.ServiceProvider.GetService<IServerManager>();

            self = new PlayerEntity(userName, Guid.NewGuid(), Enums.PlayerRoleEnum.None);
            var group = serverManager.GetNonFullRoomEntity();
            if (group == null)
            {
                var guid = Guid.NewGuid();
                self.RoomId = guid;
                (room, storage) = await Group.AddAsync(guid.ToString(), self);
                serverManager.AddGroup(guid, new RoomEntity(guid, room, storage));
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
            
            SetRoles(players);
            SetCards(players);
            Broadcast(room).OnGameStart(storage.AllValues.ToArray());
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

        private void SetRoles(List<PlayerEntity> players)
        {
            var first = players.GetRandomElement();
            first.IsDealer = true;
            first.CardPool = CreateDeck();

            var shuffled = players.Shuffle();
            shuffled[0].PlayerRole = Enums.PlayerRoleEnum.SmallBlind;
            shuffled[1].PlayerRole = Enums.PlayerRoleEnum.BigBlind;
        }

        private void SetCards(List<PlayerEntity> players)
        {
            var dealer = players.First(x => x.IsDealer);
            var deck = dealer.CardPool;
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
                players[startIndex].CardHand[0] = card1;
                players[startIndex].CardHand[1] = card2;
                dealt++;
                if (startIndex + 1 >= players.Count)
                    startIndex = 0;
                else
                    startIndex++;
            }

            dealer.CardPool = shuffled;
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

        protected override ValueTask OnDisconnected()
        {
            return ValueTask.CompletedTask;
        }
    }
}