namespace Communicator.Server.Domain.Entities;


public class Reaction
{
    public Guid Id { get; private set; }

    public string ReactionEm { get; private set; } = null!;
    public DateTime TimeAdded { get; private set; }

    public Guid MessageId { get; private set; }
    
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Message Message { get; private set; } = null!;

    private Reaction() { }

    public Reaction(
        string reaction,
        Guid messageId)
    {
        Id = Guid.NewGuid();
        ReactionEm = reaction;
        MessageId = messageId;
        TimeAdded = DateTime.UtcNow;
    }
}