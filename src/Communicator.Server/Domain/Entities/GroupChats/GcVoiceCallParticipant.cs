using Communicator.Server.Domain.Entities.Users;

namespace Communicator.Server.Domain.Entities.GroupChats;

public class GcVoiceCallParticipant
{
    public Guid UserId { get; private set; }
    public Guid GcVoiceCallId { get; private set; }

    public User User { get; private set; } = null!;
    public GcVoiceCall VoiceCall { get; private set; } = null!;

    private GcVoiceCallParticipant() { }

    public GcVoiceCallParticipant(Guid userId, Guid gcVoiceCallId)
    {
        UserId = userId;
        GcVoiceCallId = gcVoiceCallId;
    }
}