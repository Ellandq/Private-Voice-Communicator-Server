using Communicator.Server.Domain.Entities.Users;

namespace Communicator.Server.Domain.Entities.Messages;

public class DmMessage
{
    public Guid MessageId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public Message Message { get; private set; } = null!;
    public User Receiver { get; private set; } = null!;

    private DmMessage() { }

    public DmMessage(Guid messageId, Guid receiverId)
    {
        MessageId = messageId;
        ReceiverId = receiverId;
    }
}