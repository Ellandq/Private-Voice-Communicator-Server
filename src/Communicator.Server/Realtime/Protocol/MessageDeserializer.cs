using System.Text.Json;

namespace Communicator.Server.Realtime.Protocol;

public sealed class MessageDeserializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RealtimeMessage Deserialize(
        ReadOnlySpan<byte> data)
    {
        var message = JsonSerializer.Deserialize<RealtimeMessage>(
            data,
            _options);

        return message
               ?? throw new JsonException(
                   "Received an empty realtime message.");
    }
}