using Communicator.Server.Application.Messaging;
using Communicator.Server.Realtime.Protocol.Requests;

namespace Communicator.Server.Realtime.Protocol.Handlers.Implementations;

public sealed class SendMessageHandler(
    IMessageReceiver messageReceiver)
    : RealtimeMessageHandler<SendMessageRequest>
{
    public override string MessageType => 
        MessageTypes.MessageSend;
    
    public override Task HandleAsync(
        SendMessageRequest payload, 
        Guid? requestId, 
        CancellationToken cancellationToken = default)
    {
        return messageReceiver.ReceiveAsync(
            payload,
            requestId,
            cancellationToken);
    }
}