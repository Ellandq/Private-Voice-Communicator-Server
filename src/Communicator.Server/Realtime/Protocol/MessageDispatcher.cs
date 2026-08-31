using Communicator.Server.Realtime.Messaging;
using Communicator.Server.Realtime.Protocol.Handlers;

namespace Communicator.Server.Realtime.Protocol;

public sealed class MessageDispatcher(
    MessageDeserializer messageDeserializer,
    IEnumerable<IRealtimeMessageHandler> handlers)
{
    private readonly Dictionary<string, IRealtimeMessageHandler>
        _handlers = handlers.ToDictionary(
            handler => handler.MessageType,
            StringComparer.OrdinalIgnoreCase);

    public async Task DispatchAsync(
        MessageReadResult message,
        CancellationToken cancellationToken = default)
    {
        if (!message.IsText)
        {
            throw new InvalidOperationException(
                "Only text messages are supported by the realtime protocol.");
        }

        var realtimeMessage =
            messageDeserializer.Deserialize(message.Data);

        if (!_handlers.TryGetValue(
                realtimeMessage.Type,
                out var handler))
        {
            throw new InvalidOperationException(
                $"Unknown realtime message type: " +
                $"{realtimeMessage.Type}");
        }

        await handler.HandleAsync(
            realtimeMessage,
            cancellationToken);
    }
}