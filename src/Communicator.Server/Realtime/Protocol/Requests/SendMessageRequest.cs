namespace Communicator.Server.Realtime.Protocol.Requests;

public sealed record SendMessageRequest(
    Guid ConversationId,
    string Content);