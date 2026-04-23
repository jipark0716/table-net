using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TableNet.Auth.V1;
using static TableNet.WebApi.Repository.SessionExtensions;
using Service = TableNet.WebApi.Services.AuthService;

namespace TableNet.WebApi.GrpcServer;

public class AuthServiceServer(Service service) : AuthService.AuthServiceBase, IGrpcServer
{
    public override Task<LoginResult> GuestLogin(Empty _, ServerCallContext context)
    {
        Repository.Session session = service.LoginGuest();
        
        return Task.FromResult(new LoginResult
        {
            Success = new Session
            {
                Id = ByteString.CopyFrom(session.Id.Value.ToByteArray()),
            },
        });
    }

    public override Task<Empty> Logout(Empty _, ServerCallContext context)
    {
        Repository.Session session = context.GetSessionOrFail();
            
        return Task.FromResult(new Empty());
    }
}