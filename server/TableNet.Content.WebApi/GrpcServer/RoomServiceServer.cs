using Google.Protobuf;
using Grpc.Core;
using TableNet.Room.V1;
using TableNet.TypeWrapper.Validate;
using TableNet.WebApi.Repository;
using TableNet.WebApi.Vos;
using Service = TableNet.WebApi.Services.RoomService;

namespace TableNet.WebApi.GrpcServer;

public class RoomServiceServer(Service service) : RoomService.RoomServiceBase, IGrpcServer
{
    public override Task<CreateRoomResult> Create(CreateRoomRequest request, ServerCallContext context)
    {
        Session session = context.GetSessionOrFail(); 
        
        ParseResult<RoomName> roomName = RoomName.Parse(request.Name);

        if (!roomName.IsSuccess)
            return Task.FromResult(new CreateRoomResult
            {
                InvalidRequest = ProtoExtensions.CreateInvalidRequestErrors(
                    (roomName.Errors, nameof(request.Name))
                ),
            });

        RoomVo room = service.Create(roomName.Value, session);

        return Task.FromResult(new CreateRoomResult
        {
            Success = new CreateRoomSuccess
            {
                Room = new Room.V1.Room
                {
                    RoomId = ByteString.CopyFrom(room.Id.Value.ToByteArray()),
                    Name = room.Name.Value,
                },
            },
        });
    }

    public override Task<JoinRoomResult> Join(JoinRoomRequest request, ServerCallContext context)
    {
        return base.Join(request, context);
    }
}