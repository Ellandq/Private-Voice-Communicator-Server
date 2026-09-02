using System.Text.Json;

namespace Communicator.Server.Realtime.Protocol;

public sealed record RealtimeMessage(
    string Type,
    Guid? RequestId,
    JsonElement? Payload);