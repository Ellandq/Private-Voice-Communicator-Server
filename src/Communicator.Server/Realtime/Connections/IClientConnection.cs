using System.Net.WebSockets;

namespace Communicator.Server.Realtime.Connections;

public interface IClientConnection
{
    Guid Id { get; }
    
    bool IsOpen { get; }

    Task SendAsync(
        string message,
        CancellationToken cancellationToken = default);

    Task CloseAsync(
        WebSocketCloseStatus status,
        string? description = null,
        CancellationToken cancellationToken = default);
}