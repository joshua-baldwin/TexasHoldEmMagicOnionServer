using MagicOnion.Server.Hubs;
using THE.Entities;

namespace THE.ServerEntities;

public class RoomEntity
{
    public readonly Guid Id;
    public readonly List<PlayerEntity> PlayerList;
    
    //(PlayerId, ConnectionId)
    private readonly Dictionary<Guid, Guid> connections = new();

    public RoomEntity(Guid id, List<PlayerEntity> playerList)
    {
        Id = id;
        PlayerList = playerList;
    }

    public void AddConnection(Guid playerId, Guid connectionId)
    {
        connections.TryAdd(playerId, connectionId);
    }

    public void RemoveConnection(Guid playerId)
    {
        connections.Remove(playerId);
    }

    public Guid GetConnectionId(Guid playerId)
    {
        connections.TryGetValue(playerId, out var connectionId);
        return connectionId;
    }
}