namespace Communicator.Server.Domain.Entities;

public class GcIcon
{
    public Guid GroupChatId { get; private set; }
    public Guid MediaId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;
    public Media Media { get; private set; } = null!;

    private GcIcon() { }

    public GcIcon(Guid groupChatId, Guid mediaId)
    {
        GroupChatId = groupChatId;
        MediaId = mediaId;
    }
}