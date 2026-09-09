namespace Communicator.Server.Domain.Entities.Messages;

public class MessageAttachment
{
    public Guid MessageId { get; private set; }
    public Guid MediaId { get; private set; }

    public Message Message { get; private set; } = null!;
    public Media.Media Media { get; private set; } = null!;

    private MessageAttachment() { }

    public MessageAttachment(
        Guid messageId,
        Guid mediaId)
    {
        MessageId = messageId;
        MediaId = mediaId;
    }
}