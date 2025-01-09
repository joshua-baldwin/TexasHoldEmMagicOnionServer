using MagicOnion.Server.Hubs;
using THE.ServerEntities;
using THE.Entities;
using THE.GameLogic;
using THE.Shared.Utilities;

namespace THE.Managers
{
    public interface IRoomManager
    {
        void AddRoomAndConnection(Guid roomId, IInMemoryStorage<PlayerEntity> storage, Guid playerId, Guid connectionId, IGameLogicManager? gameLogicManager, IJokerManager? jokerManager);
        void AddConnection(Guid roomId, Guid playerId, Guid connectionId);
        void RemoveConnection(Guid roomId, Guid playerId);
        RoomEntity GetRoomEntity(Guid roomId);
        RoomEntity GetNonFullRoomEntity();
        void ClearRoom(Guid roomId);
        int GetRoomCount();
    }
    
    public class RoomManager : IRoomManager
    {
        public const int MaxRoomCount = 10;
        private Dictionary<Guid, RoomEntity> roomDictionary = new();

        public void AddRoomAndConnection(Guid roomId, IInMemoryStorage<PlayerEntity> storage, Guid playerId, Guid connectionId, IGameLogicManager? gameLogicManager, IJokerManager? jokerManager)
        {
            var roomEntity = new RoomEntity(roomId, storage, gameLogicManager, jokerManager);
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
                if (room.Storage.AllValues.Count < Constants.MaxPlayers && !room.Closed)
                    return room;
            }

            return null;
        }

        public void ClearRoom(Guid roomId)
        {
            roomDictionary.Remove(roomId);
        }
        
        public int GetRoomCount() => roomDictionary.Count;
    }
}