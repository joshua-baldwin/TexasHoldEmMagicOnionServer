using MagicOnion.Server.Hubs;

namespace TexasHoldEmServer.ServerManager
{
    public interface IServerManager
    {
        void AddGroup(Guid id, RoomEntity group);
        RoomEntity GetRoomEntity(Guid id);
        RoomEntity GetNonFullRoomEntity();
    }
}