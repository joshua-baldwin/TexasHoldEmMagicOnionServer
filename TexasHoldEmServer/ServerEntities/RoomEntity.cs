using MagicOnion.Server.Hubs;
using THE.Entities;
using THE.GameLogic;
using THE.Shared.Utilities;

namespace THE.ServerEntities;

public class RoomEntity
{
    public readonly Guid Id;
    public readonly IInMemoryStorage<PlayerEntity> Storage;
    public readonly IGameLogicManager? GameLogicManager;
    public readonly IJokerManager? JokerManager;
    public bool Closed { get; private set; }
    
    //(PlayerId, ConnectionId)
    private readonly Dictionary<Guid, Guid> connections = new();

    public RoomEntity(Guid id, IInMemoryStorage<PlayerEntity> storage, IGameLogicManager? gameLogicManager, IJokerManager? jokerManager)
    {
        Id = id;
        Storage = storage;
        GameLogicManager = gameLogicManager;
        JokerManager = jokerManager;
    }

    public void AddConnection(Guid playerId, Guid connectionId)
    {
        connections.TryAdd(playerId, connectionId);
        if (connections.Count == Constants.MaxPlayers)
            Closed = true;
    }

    public void RemoveConnection(Guid playerId)
    {
        connections.Remove(playerId);
        if (connections.Count < Constants.MaxPlayers)
            Closed = false;
    }

    public Guid GetConnectionId(Guid playerId)
    {
        connections.TryGetValue(playerId, out var connectionId);
        return connectionId;
    }

    public void CloseRoom()
    {
        Closed = true;
    }
}