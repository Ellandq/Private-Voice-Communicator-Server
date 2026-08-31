using System.Net.WebSockets;
using System.Text;

namespace Communicator.Server.Realtime.Connections;

public sealed class WebSocketClientConnection(
    Guid id,
    WebSocket webSocket) : IClientConnection
{
    public Guid Id { get; } = id;

    public bool IsOpen => 
        webSocket.State == WebSocketState.Open;

    public async Task SendAsync(
        string message, 
        CancellationToken cancellationToken = default)
    {
        var data = Encoding.UTF8.GetBytes(message);

        await webSocket.SendAsync(
            data,
            WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken);
    }

    public async Task CloseAsync(
        WebSocketCloseStatus status, 
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        if (webSocket.State is WebSocketState.Closed
            or WebSocketState.Aborted)
        {
            return;
        }

        await webSocket.CloseAsync(
            status,
            description,
            cancellationToken);
    }
}