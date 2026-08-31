using System.Net.WebSockets;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;

namespace Communicator.Server.Realtime;

public sealed class WebSocketHandler(
    ConnectionManager connectionManager,
    WebSocketMessageReader webSocketMessageReader)
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
                await connection.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection closed by client.",
                    cancellationToken);
                
                return;
            }
            
            // TODO
        }
    }


}