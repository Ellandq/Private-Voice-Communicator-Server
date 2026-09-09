namespace Communicator.Server.Domain.Entities;


public class GcVoiceCall
{
    public Guid Id { get; private set; }
    public Guid GroupChatId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public ICollection<GcVoiceCallParticipant> Participants { get; private set; }
        = new List<GcVoiceCallParticipant>();

    private GcVoiceCall() { }

    public GcVoiceCall(Guid groupChatId)
    {
        Id = Guid.NewGuid();
        GroupChatId = groupChatId;
    }
}