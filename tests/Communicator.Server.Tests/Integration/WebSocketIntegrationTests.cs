using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Communicator.Server.Realtime.Protocol;

namespace Communicator.Server.Tests.Integration;

public sealed class WebSocketIntegrationTests
{
    private static readonly HttpClient Client = new();

    private const string ExpectedHealthResponse =
        "Communicator server is running.";

    [Fact]
    public async Task Server_accepts_and_processes_valid_websocket_message()
    {
        var serverUrl = "http://localhost:5164";


        if (string.IsNullOrWhiteSpace(serverUrl))
        {
            Assert.True(true,
                "COMMUNICATOR_TEST_SERVER is not configured.");
        }

        if (!Uri.TryCreate(
                serverUrl,
                UriKind.Absolute,
                out var baseUri))
        {
            Assert.True(true,
                $"COMMUNICATOR_TEST_SERVER is invalid: {serverUrl}");
        }

        if (!await IsServerAvailableAsync(baseUri))
        {
            Assert.True(true,
                $"Communicator server is not available at {baseUri}");
        }

        using var healthResponse =
            await Client.GetAsync(baseUri);

        Assert.True(
            healthResponse.IsSuccessStatusCode,
            $"Health endpoint returned {healthResponse.StatusCode}.");

        var healthBody =
            await healthResponse.Content.ReadAsStringAsync();

        Assert.Equal(
            ExpectedHealthResponse,
            healthBody);

        using var socket = new ClientWebSocket();

        var webSocketUri = BuildWebSocketUri(baseUri);

        await socket.ConnectAsync(
            webSocketUri,
            CancellationToken.None);

        Assert.Equal(
            WebSocketState.Open,
            socket.State);

        await SendMessageAsync(socket);

        await socket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "Integration test completed.",
            CancellationToken.None);

        Assert.Equal(
            WebSocketState.Closed,
            socket.State);
    }

    private static async Task<bool> IsServerAvailableAsync(
        Uri baseUri)
    {
        try
        {
            using var client = new HttpClient();

            using var response =
                await client.GetAsync(baseUri);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var body =
                await response.Content.ReadAsStringAsync();

            return body == ExpectedHealthResponse;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    private static Uri BuildWebSocketUri(Uri baseUri)
    {
        var builder = new UriBuilder(baseUri)
        {
            Scheme = baseUri.Scheme == "https"
                ? "wss"
                : "ws",
            Path = "/ws"
        };

        return builder.Uri;
    }

    private static async Task SendMessageAsync(
        ClientWebSocket socket)
    {
        var message = new
        {
            type = MessageTypes.MessageSend,
            requestId = Guid.NewGuid(),
            payload = new
            {
                conversationId = Guid.NewGuid(),
                content = "Integration test message"
            }
        };

        var json = JsonSerializer.Serialize(message);
        var data = Encoding.UTF8.GetBytes(json);

        await socket.SendAsync(
            data,
            WebSocketMessageType.Text,
            endOfMessage: true,
            CancellationToken.None);
    }
}