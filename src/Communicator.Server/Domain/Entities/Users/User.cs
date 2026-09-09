using Communicator.Server.Domain.Entities.GroupChats;
using Communicator.Server.Domain.Entities.Messages;
using Communicator.Server.Domain.Enums;

namespace Communicator.Server.Domain.Entities.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = null!;
    public UserStatus Status { get; private set; }
    public string? StatusMessage { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    
    public ProfilePicture? ProfilePicture { get; private set; }

    public ICollection<GroupChatMember> GroupChats { get; private set; }
        = new List<GroupChatMember>();

    public ICollection<GcVoiceCallParticipant> VoiceCalls { get; private set; }
        = new List<GcVoiceCallParticipant>();

    public ICollection<Message> SentMessages { get; private set; }
        = new List<Message>();

    public ICollection<DmMessage> ReceivedDirectMessages { get; private set; }
        = new List<DmMessage>();

    public ICollection<Media.Media> OwnedMedia { get; private set; }
        = new List<Media.Media>();

    private User() { }

    public User(
        string username,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Status = UserStatus.Offline;
        CreatedAt = DateTime.UtcNow;
    }
}