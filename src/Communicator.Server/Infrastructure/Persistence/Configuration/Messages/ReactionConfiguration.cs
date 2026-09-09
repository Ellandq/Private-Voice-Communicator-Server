using Communicator.Server.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Messages;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.ReactionEm)
            .IsRequired()
            .HasMaxLength(16);

        entity.HasOne(x => x.Message)
            .WithMany(x => x.Reactions)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => new
        {
            x.MessageId,
            x.UserId,
            x.ReactionEm
        }).IsUnique();

        entity.HasIndex(x => new
        {
            x.MessageId,
            x.TimeAdded
        });
    }
}