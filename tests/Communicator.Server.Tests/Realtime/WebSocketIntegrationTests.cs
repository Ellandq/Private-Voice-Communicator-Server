using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Communicator.Server.Tests.Realtime;

public class WebSocketIntegrationTests
{
    [Fact]
    public async Task WebSocket_ClientCanConnect()
    {
        await using var factory =
            new WebApplicationFactory<Program>();

        var client =
            factory.Server.CreateWebSocketClient();

        using var webSocket =
            await client.ConnectAsync(
                new Uri("ws://localhost/ws"),
                CancellationToken.None);

        Assert.Equal(
            WebSocketState.Open,
            webSocket.State);
    }

    [Fact]
    public async Task WebSocket_ClientCanCloseConnection()
    {
        await using var factory =
            new WebApplicationFactory<Program>();

        var client =
            factory.Server.CreateWebSocketClient();

        using var webSocket =
            await client.ConnectAsync(
                new Uri("ws://localhost/ws"),
                CancellationToken.None);

        await webSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "Test completed.",
            CancellationToken.None);

        Assert.Equal(
            WebSocketState.Closed,
            webSocket.State);
    }
}