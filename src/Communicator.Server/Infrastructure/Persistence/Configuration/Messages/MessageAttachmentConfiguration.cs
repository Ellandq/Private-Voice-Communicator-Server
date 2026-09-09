using Communicator.Server.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Messages;

public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> entity)
    {
        entity.HasKey(x => new
        {
            x.MessageId,
            x.MediaId
        });

        entity.HasOne(x => x.Message)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Media)
            .WithMany(x => x.MessageAttachments)
            .HasForeignKey(x => x.MediaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}