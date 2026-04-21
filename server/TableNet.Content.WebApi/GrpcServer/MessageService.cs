using Grpc.Core;
using TableNet.Chat.V1;

namespace TableNet.WebApi.GrpcServer;

public class MessageService : TableNet.Chat.V1.MessageService.MessageServiceBase
{
    public override Task<CreateMessageResult> CreateMessage(CreateMessageRequest request, ServerCallContext context)
    {
        return base.CreateMessage(request, context);
    }

    public override Task ReceiveMessage(Receive request, IServerStreamWriter<MessageEvent> responseStream, ServerCallContext context)
    {
        return base.ReceiveMessage(request, responseStream, context);
    }
}