using Grpc.Core;
using TableNet.Room.V1;

namespace TableNet.WebApi.GrpcServer;

public class RoomServiceServer : RoomService.RoomServiceBase
{
    public override Task<CreateRoomResult> Create(CreateRoomRequest request, ServerCallContext context)
    {
        return base.Create(request, context);
    }

    public override Task<JoinRoomResult> Join(JoinRoomRequest request, ServerCallContext context)
    {
        return base.Join(request, context);
    }
}