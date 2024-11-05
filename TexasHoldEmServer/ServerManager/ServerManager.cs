using MagicOnion.Server.Hubs;

namespace TexasHoldEmServer.ServerManager
{
    public class ServerManager : IServerManager
    {
        private Dictionary<Guid, RoomEntity> groups;
        public ServerManager()
        {
            groups = new Dictionary<Guid, RoomEntity>();
        }

        public void AddGroup(Guid id, RoomEntity group)
        {
            groups.TryAdd(id, group);
        }

        public RoomEntity GetRoomEntity(Guid id)
        {
            groups.TryGetValue(id, out var group);
            return group;
        }

        public RoomEntity GetNonFullRoomEntity()
        {
            foreach (var room in groups.Values)
            {
                if (room.Storage.AllValues.Count < 10)
                    return room;
            }

            return null;
        }
    }
}