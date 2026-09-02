using System.Text.Json;
using Communicator.Server.Realtime.Protocol;
using Communicator.Server.Realtime.Protocol.Handlers;
using Communicator.Server.Realtime.Protocol.Requests;

namespace Communicator.Server.Tests.Realtime.Protocol;

public sealed class RealtimeMessageHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidPayload_DeserializesAndInvokesTypedHandler()
    {
        var handler = new TestHandler();

        var message = new RealtimeMessage(
            "message.test",
            Guid.NewGuid(),
            JsonSerializer.SerializeToElement(
                new
                {
                    ConversationId = Guid.NewGuid(),
                    Content = "Hello"
                }));

        await handler.HandleAsync(message);

        Assert.NotNull(handler.ReceivedPayload);
        Assert.Equal(
            "Hello",
            handler.ReceivedPayload.Content);
    }

    [Fact]
    public async Task HandleAsync_MissingPayload_Throws()
    {
        var handler = new TestHandler();

        var message = new RealtimeMessage(
            "message.test",
            null,
            null);

        await Assert.ThrowsAsync<JsonException>(
            () => handler.HandleAsync(message));
    }

    private sealed class TestHandler
        : RealtimeMessageHandler<SendMessageRequest>
    {
        public override string MessageType =>
            "message.test";

        public SendMessageRequest? ReceivedPayload { get; private set; }

        public override Task HandleAsync(
            SendMessageRequest payload,
            Guid? requestId,
            CancellationToken cancellationToken = default)
        {
            ReceivedPayload = payload;

            return Task.CompletedTask;
        }
    }
}