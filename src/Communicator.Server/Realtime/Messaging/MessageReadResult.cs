using System.Net.WebSockets;

namespace Communicator.Server.Realtime.Messaging;

public sealed record MessageReadResult(
    WebSocketMessageType MessageType,
    byte[] Data)
{
    public bool IsText =>
        MessageType == WebSocketMessageType.Text;

    public bool IsBinary =>
        MessageType == WebSocketMessageType.Binary;
}