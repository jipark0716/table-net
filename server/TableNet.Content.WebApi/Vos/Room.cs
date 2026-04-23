using System.Collections.Concurrent;

namespace TableNet.WebApi.Vos;

public readonly record struct RoomVo(RoomId Id, RoomName Name, ConcurrentDictionary<SessionId, byte> Sessions);