using Communicator.Server.Domain.Entities.Messages;

namespace Communicator.Server.Domain.Entities.GroupChats;


public class GcMessage
{
    public Guid MessageId { get; private set; }
    public Guid GroupChatId { get; private set; }

    public Message Message { get; private set; } = null!;
    public GroupChat GroupChat { get; private set; } = null!;

    private GcMessage() { }

    public GcMessage(Guid messageId, Guid groupChatId)
    {
        MessageId = messageId;
        GroupChatId = groupChatId;
    }
}