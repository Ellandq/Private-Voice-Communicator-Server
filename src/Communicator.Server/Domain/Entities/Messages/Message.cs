using Communicator.Server.Domain.Entities.GroupChats;
using Communicator.Server.Domain.Entities.Users;

namespace Communicator.Server.Domain.Entities.Messages;

public class Message
{
    public Guid Id { get; private set; }

    public string CipherText { get; private set; } = null!;
    public DateTime TimeSent { get; private set; }

    public Guid SenderId { get; private set; }

    public User Sender { get; private set; } = null!;

    public GcMessage? GroupChatMessage { get; private set; }
    public DmMessage? DirectMessage { get; private set; }

    public ICollection<Reaction> Reactions { get; private set; }
        = new List<Reaction>();

    public ICollection<MessageAttachment> Attachments { get; private set; }
        = new List<MessageAttachment>();

    private Message() { }

    public Message(
        string cipherText,
        Guid senderId)
    {
        Id = Guid.NewGuid();
        CipherText = cipherText;
        SenderId = senderId;
        TimeSent = DateTime.UtcNow;
    }
}