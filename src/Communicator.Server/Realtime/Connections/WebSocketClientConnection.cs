using System.Net.WebSockets;
using System.Text;
using Communicator.Server.Realtime.Messaging;

namespace Communicator.Server.Realtime.Connections;

public sealed class WebSocketClientConnection(
    WebSocket webSocket, WebSocketMessageReader messageReader)
    : IClientConnection, IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();

    public bool IsOpen => 
        webSocket.State == WebSocketState.Open;

    public async Task<MessageReadResult?> ReceiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await messageReader.ReadAsync(
            webSocket,
            cancellationToken);
    }
    
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

    public void Dispose()
    {
        webSocket.Dispose();
    }
}