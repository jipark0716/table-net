using System.Collections.Concurrent;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.Services;

public class SessionFarm
{
    private readonly ConcurrentDictionary<SessionId, Session> _sessions = [];

    public Session CreateSession()
    {
        throw new NotImplementedException();
    }

    public Session? GetSession(SessionId id)
    {
        _sessions.TryGetValue(id, out Session? session);

        return session;
    }

    public bool DeleteSession(SessionId id)
    {
        Session? session = GetSession(id);
        
        session?.Dispose();

        return session is not null;
    }
}

public record Session : IDisposable
{
    public void Dispose()
    {
        // TODO 관리되는 리소스를 여기에 릴리스
    }
}