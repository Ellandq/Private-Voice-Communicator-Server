namespace Communicator.Server.Domain.Entities.GroupChats;

public class GroupChat
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public ICollection<GroupChatMember> Members { get; private set; }
        = new List<GroupChatMember>();

    public ICollection<GcVoiceCall> VoiceCalls { get; private set; }
        = new List<GcVoiceCall>();

    public ICollection<GcMessage> Messages { get; private set; }
        = new List<GcMessage>();

    public GcIcon? Icon { get; private set; }

    private GroupChat() { }

    public GroupChat(string name, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}