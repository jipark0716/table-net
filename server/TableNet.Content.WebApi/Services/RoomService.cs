using TableNet.WebApi.Repository;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.Services;

public class RoomService(RoomRepository repository) : IScoped
{
    public RoomVo Create(RoomName name, Session session)
    {
        RoomVo room = new(new RoomId(Guid.NewGuid()), name, []);
        
        room.Sessions.TryAdd(session.Id, 0);

        repository.Create(room);

        return room;
    }

    public bool Join(RoomId id, Session session)
    {
        if (repository.TryGet(id, out RoomVo room))
        {
            return false;
        }

        room.Sessions.TryAdd(session.Id, 0);

        return true;
    }
}