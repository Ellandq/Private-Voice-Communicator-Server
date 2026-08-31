using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Communicator.Server.Tests.Realtime;

public class WebSocketHandlerTests
{
    [Fact]
    public async Task WebSocket_ReceivesAndSendsResponse()
    {
        await using var factory =
            new WebApplicationFactory<Program>();

        var client =
            factory.Server.CreateWebSocketClient();

        using var webSocket =
            await client.ConnectAsync(
                new Uri("ws://localhost/ws"),
                CancellationToken.None);

        const string message = "Test Message.";

        var messageBytes =
            Encoding.UTF8.GetBytes(message);

        await webSocket.SendAsync(
            new ArraySegment<byte>(messageBytes),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);

        var buffer = new byte[4096];

        var result =
            await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None);

        var response =
            Encoding.UTF8.GetString(
                buffer,
                0,
                result.Count);
        
        Assert.Equal(
            "Server received: Test Message.",
            response);

    }
}