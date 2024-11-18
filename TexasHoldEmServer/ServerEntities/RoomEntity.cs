using MagicOnion.Server.Hubs;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.ServerEntities;

public class RoomEntity
{
    public readonly Guid Id;
    public readonly IGroup Group;
    public readonly IInMemoryStorage<PlayerEntity> Storage;
    
    //(PlayerId, ConnectionId)
    private readonly Dictionary<Guid, Guid> connections = new();

    public RoomEntity(Guid id, IGroup group, IInMemoryStorage<PlayerEntity> storage)
    {
        Id = id;
        Group = group;
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
}