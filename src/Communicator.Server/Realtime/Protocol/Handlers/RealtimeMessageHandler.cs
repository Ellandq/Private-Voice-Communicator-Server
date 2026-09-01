using System.Text.Json;

namespace Communicator.Server.Realtime.Protocol.Handlers;

public abstract class RealtimeMessageHandler<TPayload>
    : IRealtimeMessageHandler<TPayload>
{
    public abstract string MessageType { get; }
    
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public async Task HandleAsync(
        RealtimeMessage message, 
        CancellationToken cancellationToken = default)
    {
        if (message.Payload is null)
        {
            throw new JsonException(
                $"Message '{message.Type}' requires a payload");
        }

        var payload = message.Payload.Value
            .Deserialize<TPayload>(_jsonOptions);

        if (payload is null)
        {
            throw new JsonException(
                $"Could not deserialize payload for '{message.Type}'.");
        }

        await HandleAsync(
            payload,
            message.RequestId,
            cancellationToken);
    }

    public abstract Task HandleAsync(
        TPayload payload,
        Guid? requestId,
        CancellationToken cancellationToken = default);
}