using Communicator.Server.Domain.Entities.GroupChats;
using Communicator.Server.Domain.Entities.Messages;
using Communicator.Server.Domain.Entities.Users;

namespace Communicator.Server.Domain.Entities.Media;

public class Media
{
    public Guid Id { get; private set; }

    public Guid OwnerId { get; private set; }

    public string StorageKey { get; private set; } = null!;
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long Size { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public User Owner { get; private set; } = null!;

    public ICollection<MessageAttachment> MessageAttachments { get; private set; }
        = new List<MessageAttachment>();

    public ProfilePicture? ProfilePicture { get; private set; }
    public GcIcon? GroupChatIcon { get; private set; }

    private Media() { }

    public Media(
        Guid ownerId,
        string storageKey,
        string fileName,
        string contentType,
        long size)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        StorageKey = storageKey;
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        CreatedAt = DateTime.UtcNow;
    }
}