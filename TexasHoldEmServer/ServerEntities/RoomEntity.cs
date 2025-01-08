using MagicOnion.Server.Hubs;
using THE.Entities;

namespace THE.ServerEntities;

public class RoomEntity
{
    public readonly Guid Id;
    public readonly IInMemoryStorage<PlayerEntity> Storage;
    
    //(PlayerId, ConnectionId)
    private readonly Dictionary<Guid, Guid> connections = new();

    public RoomEntity(Guid id, IInMemoryStorage<PlayerEntity> storage)
    {
        Id = id;
        Storage = storage;
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