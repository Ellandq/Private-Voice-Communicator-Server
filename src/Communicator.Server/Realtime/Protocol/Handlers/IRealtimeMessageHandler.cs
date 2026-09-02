namespace Communicator.Server.Realtime.Protocol.Handlers;

public interface IRealtimeMessageHandler
{
    string MessageType { get; }

    Task HandleAsync(
        RealtimeMessage message,
        CancellationToken cancellationToken = default);
}

public interface IRealtimeMessageHandler<in TPayload>
    : IRealtimeMessageHandler
{
    Task HandleAsync(
        TPayload payload,
        Guid? requestId,
        CancellationToken cancellationToken = default);
}