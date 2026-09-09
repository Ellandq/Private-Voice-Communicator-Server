namespace Communicator.Server.Domain.Entities.Users;

public class ProfilePicture
{
    public Guid UserId { get; private set; }
    public Guid MediaId { get; private set; }

    public User User { get; private set; } = null!;
    public Media.Media Media { get; private set; } = null!;

    private ProfilePicture() { }

    public ProfilePicture(Guid userId, Guid mediaId)
    {
        UserId = userId;
        MediaId = mediaId;
    }
}