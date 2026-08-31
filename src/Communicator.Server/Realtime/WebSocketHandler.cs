using System.Net.WebSockets;
using System.Text;

namespace Communicator.Server.Realtime;

public static class WebSocketHandler
{
    public static async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        using var webSocket =
            await context.WebSockets.AcceptWebSocketAsync();
        
        Console.WriteLine("WebSocket client connected.");

        await ReceiveMessagesAsync(webSocket);
    }

    private static async Task ReceiveMessagesAsync(WebSocket webSocket)
    {
        var buffer = new byte[4096];

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult? result = null;
            
            using var ms = new MemoryStream();
            do
            {
                result = await webSocket.ReceiveAsync(
                    buffer,
                    CancellationToken.None);

                ms.Write(
                    buffer,
                    0,
                    result.Count);
                
            } while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);
                        
            switch (result.MessageType)
            {
                case WebSocketMessageType.Close:
                    await CloseConnectionAsync(webSocket, "User requested socket closure.");
                    return;
                
                case WebSocketMessageType.Text:
                {
                    using var reader = new StreamReader(ms, Encoding.UTF8);
                    var message = await reader.ReadToEndAsync();
                        
                    Console.WriteLine($"Received: {message}");
                
                    await SendMessageAsync(
                        webSocket, 
                        $"Server received: {message}");
                    break;
                }
                
                case WebSocketMessageType.Binary:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static async Task SendMessageAsync(
        WebSocket webSocket,
        string message)
    {
        var data = Encoding.UTF8.GetBytes(message);

        await webSocket.SendAsync(
            data,
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    private static async Task CloseConnectionAsync(
        WebSocket webSocket,
        string message)
    {
        Console.WriteLine("WebSocket client disconnected.");

        await webSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            message,
            CancellationToken.None);
    }
}