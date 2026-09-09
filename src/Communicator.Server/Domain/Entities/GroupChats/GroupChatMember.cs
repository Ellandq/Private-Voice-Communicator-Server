using Communicator.Server.Domain.Entities.Users;

namespace Communicator.Server.Domain.Entities.GroupChats;

public class GroupChatMember
{
    public Guid GroupChatId { get; private set; }
    public Guid UserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private GroupChatMember() { }

    public GroupChatMember(Guid groupChatId, Guid userId)
    {
        GroupChatId = groupChatId;
        UserId = userId;
    }
}