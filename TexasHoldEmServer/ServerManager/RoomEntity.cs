using MagicOnion.Server.Hubs;
using THE.MagicOnion.Shared.Entities;

namespace TexasHoldEmServer.ServerManager;

public class RoomEntity
{
    public Guid Id;
    public IGroup Group;
    public IInMemoryStorage<PlayerEntity> Storage;

    public RoomEntity(Guid id, IGroup group, IInMemoryStorage<PlayerEntity> storage)
    {
        Id = id;
        Group = group;
        Storage = storage;
    }
}