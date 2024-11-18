using MagicOnion.Server.Hubs;
using TexasHoldEmServer.GameLogic;
using TexasHoldEmServer.ServerEntities;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.Managers
{
    public interface IRoomManager
    {
        void AddRoomAndConnection(Guid roomId, IGroup? group, IInMemoryStorage<PlayerEntity> storage, Guid playerId, Guid connectionId);
        void AddConnection(Guid roomId, Guid playerId, Guid connectionId);
        void RemoveConnection(Guid roomId, Guid playerId);
        RoomEntity GetRoomEntity(Guid roomId);
        RoomEntity GetNonFullRoomEntity();
    }
    
    public class RoomManager : IRoomManager
    {
        private Dictionary<Guid, RoomEntity> roomDictionary = new();

        public void AddRoomAndConnection(Guid roomId, IGroup? group, IInMemoryStorage<PlayerEntity> storage, Guid playerId, Guid connectionId)
        {
            var roomEntity = new RoomEntity(roomId, group, storage);
            roomDictionary.TryAdd(roomId, roomEntity);
            AddConnection(roomId, playerId, connectionId);
        }

        public void AddConnection(Guid roomId, Guid playerId, Guid connectionId)
        {
            roomDictionary.TryGetValue(roomId, out var roomEntity);
            roomEntity?.AddConnection(playerId, connectionId);
        }

        public void RemoveConnection(Guid roomId, Guid playerId)
        {
            roomDictionary.TryGetValue(roomId, out var roomEntity);
            roomEntity?.RemoveConnection(playerId);
        }

        public RoomEntity GetRoomEntity(Guid roomId)
        {
            roomDictionary.TryGetValue(roomId, out var room);
            return room;
        }

        public RoomEntity GetNonFullRoomEntity()
        {
            foreach (var room in roomDictionary.Values)
            {
                if (room.Storage.AllValues.Count < GameLogicManager.MaxPlayers)
                    return room;
            }

            return null;
        }
    }
}