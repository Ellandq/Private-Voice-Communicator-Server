using System.Net.WebSockets;

namespace Communicator.Server.Realtime.Messaging;

public sealed class WebSocketMessageReader
{
    private const int BufferSize = 4096;

    public async Task<MessageReadResult?> ReadAsync(
        WebSocket webSocket,
        CancellationToken cancellationToken = default)
    {
        var buffer = new byte[BufferSize];

        using var messageStream = new MemoryStream();

        WebSocketReceiveResult result;

        do
        {
            result = await webSocket.ReceiveAsync(
                buffer,
                cancellationToken);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                return null;
            }

            messageStream.Write(
                buffer,
                0,
                result.Count);
        } while (!result.EndOfMessage);

        return new MessageReadResult(
            result.MessageType,
            messageStream.ToArray());
    }
}