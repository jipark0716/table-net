using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TableNet.Chat.V1;
using TableNet.TypeWrapper.Validate;
using TableNet.WebApi.Vos;
using Service = TableNet.WebApi.Services.MessageService;

namespace TableNet.WebApi.GrpcServer;

public class MessageServiceServer(Service service) : MessageService.MessageServiceBase, IGrpcServer
{
    public override Task<CreateMessageResult> CreateMessage(CreateMessageRequest request, ServerCallContext context)
    {
        ParseResult<MessageContent> messageContent = MessageContent.Parse(request.Message.Content);

        if (!messageContent.IsSuccess)
            return Task.FromResult(new CreateMessageResult
            {
                InvalidRequest = ProtoExtensions.CreateInvalidRequestErrors(
                    (messageContent.Errors, nameof(request.Message.Content))
                ),
            });

        service.CreateMessage(messageContent.Value);
        
        return Task.FromResult(new CreateMessageResult
        {
            Success = new Empty(),
        });
    }

    public override Task ReceiveMessage(Empty request, IServerStreamWriter<MessageEvent> responseStream,
        ServerCallContext context)
    {
        return base.ReceiveMessage(request, responseStream, context);
    }
}