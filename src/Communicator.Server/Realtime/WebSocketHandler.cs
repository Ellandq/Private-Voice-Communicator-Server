using System.Net.WebSockets;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;
using Communicator.Server.Realtime.Protocol;

namespace Communicator.Server.Realtime;

public sealed class WebSocketHandler(
    ConnectionManager connectionManager,
    WebSocketMessageReader webSocketMessageReader,
    MessageDispatcher messageDispatcher)
{
    public async Task HandleAsync(
        WebSocket webSocket,
        CancellationToken cancellationToken)
    {
        var connection = new WebSocketClientConnection(
            webSocket, 
            webSocketMessageReader);

        connectionManager.AddConnection(connection);

        try
        {
            await ReceiveMessagesAsync(
                connection,
                cancellationToken);
        }
        finally
        {
            connectionManager.RemoveConnection(connection.Id);
            
            connection.Dispose();
        }
    }

    private async Task ReceiveMessagesAsync(
        IClientConnection connection,
        CancellationToken cancellationToken)
    {
        while (connection.IsOpen)
        {
            var result = await connection.ReceiveAsync(
                cancellationToken);

            if (result is null)
            {
                return;
            }

            await messageDispatcher.DispatchAsync(
                result,
                cancellationToken);
        }
    }


}