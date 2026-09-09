using Communicator.Server.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Messages;

public class DmMessageConfiguration : IEntityTypeConfiguration<DmMessage>
{
    public void Configure(EntityTypeBuilder<DmMessage> entity)
    {
        entity.HasKey(x => x.MessageId);

        entity.HasOne(x => x.Message)
            .WithOne(x => x.DirectMessage)
            .HasForeignKey<DmMessage>(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Receiver)
            .WithMany(x => x.ReceivedDirectMessages)
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}