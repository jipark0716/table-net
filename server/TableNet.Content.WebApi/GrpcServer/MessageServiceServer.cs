using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TableNet.Chat.V1;
using TableNet.TypeWrapper.Validate;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.GrpcServer;

public class MessageServiceServer : MessageService.MessageServiceBase
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
        
        // todo message 발송

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