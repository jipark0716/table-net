using Grpc.Core;
using Grpc.Core.Interceptors;
using TableNet.WebApi.Repository;
using TableNet.WebApi.Services;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi;

public class AuthInterceptor(SessionFarm sessionFarm) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (!context.UserState.ContainsKey("session"))
        {
            string? auth = context.RequestHeaders.GetValue("authorization");

            if (Guid.TryParse(auth, out Guid sessionId) && sessionFarm.TryGetSession(new SessionId(sessionId), out Session? session))
            {
                context.UserState["session"] = session;
            }
        }
        
        return await continuation(request, context);
    }
}