using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Messages;

public class GcMessageConfiguration : IEntityTypeConfiguration<GcMessage>
{
    public void Configure(EntityTypeBuilder<GcMessage> entity)
    {
        entity.HasKey(x => x.MessageId);

        entity.HasOne(x => x.Message)
            .WithOne(x => x.GroupChatMessage)
            .HasForeignKey<GcMessage>(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.GroupChat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(x => new
        {
            x.GroupChatId,
            x.MessageId
        });
    }
}