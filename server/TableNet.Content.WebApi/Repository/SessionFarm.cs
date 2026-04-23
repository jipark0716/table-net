using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.Repository;

public class SessionFarm : ISingleton
{
    private readonly ConcurrentDictionary<SessionId, Session> _sessions = [];

    public bool CreateSession(Session session) => _sessions.TryAdd(session.Id, session);

    public bool TryGetSession(SessionId id, [NotNullWhen(true)] out Session? session) => _sessions.TryGetValue(id, out session);

    public bool DeleteSession(SessionId id)
    {
        return _sessions.TryRemove(id, out _);
    }
}

public record Session
{
    public required SessionId Id { get; init; }
}

public static class SessionExtensions
{
    public static Session GetSessionOrFail(this ServerCallContext context) =>
        context.UserState["session"] as Session ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "인증 실패"));
}