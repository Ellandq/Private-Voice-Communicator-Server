using System.Net.WebSockets;
using Communicator.Server.Realtime.Messaging;

namespace Communicator.Server.Realtime.Connections;

public interface IClientConnection
{
    Guid Id { get; }
    
    bool IsOpen { get; }

    Task<MessageReadResult?> ReceiveAsync(
        CancellationToken cancellationToken = default); 

    Task SendAsync(
        string message,
        CancellationToken cancellationToken = default);

    Task CloseAsync(
        WebSocketCloseStatus status,
        string? description = null,
        CancellationToken cancellationToken = default);
}