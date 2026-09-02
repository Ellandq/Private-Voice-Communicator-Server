using System.Net.WebSockets;

namespace Communicator.Server.Realtime.Messaging;

public sealed record MessageReadResult(
    WebSocketMessageType MessageType,
    byte[] Data,
    WebSocketCloseStatus? CloseStatus = null,
    string? CloseDescription = null)
{
    public bool IsText =>
        MessageType == WebSocketMessageType.Text;

    public bool IsBinary =>
        MessageType == WebSocketMessageType.Binary;

    public bool IsClose =>
        MessageType == WebSocketMessageType.Close;
}