using Communicator.Server.Realtime.Protocol.Requests;

namespace Communicator.Server.Application.Messaging;

public interface IMessageReceiver
{
    Task ReceiveAsync(
        SendMessageRequest request,
        Guid? requestId,
        CancellationToken cancellationToken = default);
}