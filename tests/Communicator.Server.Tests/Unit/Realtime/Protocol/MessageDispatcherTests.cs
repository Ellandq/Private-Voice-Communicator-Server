using System.Text;
using Communicator.Server.Realtime.Messaging;
using Communicator.Server.Realtime.Protocol;
using Communicator.Server.Realtime.Protocol.Handlers;

namespace Communicator.Server.Tests.Realtime.Protocol;

public sealed class MessageDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_ValidMessage_CallsMatchingHandler()
    {
        var handler = new TestMessageHandler(
            "message.test");

        var dispatcher = new MessageDispatcher(
            new MessageDeserializer(),
            [handler]);

        var message = CreateMessage(
            """
            {
                "type": "message.test",
                "requestId": "11111111-1111-1111-1111-111111111111"
            }
            """);

        await dispatcher.DispatchAsync(message);

        Assert.True(handler.WasCalled);
    }

    [Fact]
    public async Task DispatchAsync_UnknownMessageType_Throws()
    {
        var dispatcher = new MessageDispatcher(
            new MessageDeserializer(),
            []);

        var message = CreateMessage(
            """
            {
                "type": "message.unknown"
            }
            """);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.DispatchAsync(message));

        Assert.Contains(
            "message.unknown",
            exception.Message);
    }

    [Fact]
    public async Task DispatchAsync_BinaryMessage_Throws()
    {
        var dispatcher = new MessageDispatcher(
            new MessageDeserializer(),
            []);

        var message = new MessageReadResult(
            System.Net.WebSockets.WebSocketMessageType.Binary,
            [1, 2, 3]);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.DispatchAsync(message));

        Assert.Contains(
            "Only text messages",
            exception.Message);
    }

    private static MessageReadResult CreateMessage(string json)
    {
        return new MessageReadResult(
            System.Net.WebSockets.WebSocketMessageType.Text,
            Encoding.UTF8.GetBytes(json));
    }

    private sealed class TestMessageHandler(
        string messageType) : IRealtimeMessageHandler
    {
        public string MessageType => messageType;

        public bool WasCalled { get; private set; }

        public Task HandleAsync(
            RealtimeMessage message,
            CancellationToken cancellationToken = default)
        {
            WasCalled = true;

            return Task.CompletedTask;
        }
    }
}