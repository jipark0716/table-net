using System.Collections.Concurrent;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.Repository;

public class RoomRepository : ISingleton
{
    private readonly ConcurrentDictionary<RoomId, RoomVo> _rooms = [];
    
    public bool Create(RoomVo room) => _rooms.TryAdd(room.Id, room);
    
    public bool TryGet(RoomId id, out RoomVo room) => _rooms.TryGetValue(id, out room);

}