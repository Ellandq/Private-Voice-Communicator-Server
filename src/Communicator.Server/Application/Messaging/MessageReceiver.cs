using Communicator.Server.Realtime.Protocol.Requests;

namespace Communicator.Server.Application.Messaging;

public sealed class MessageReceiver
    : IMessageReceiver
{
    public Task ReceiveAsync(
        SendMessageRequest request,
        Guid? requestId,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine(
            $"Send message request: " +
            $"Conversation={request.ConversationId}, " +
            $"Content={request.Content}");

        return Task.CompletedTask;
    }
}