using Communicator.Server.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Messages;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.CipherText)
            .IsRequired();

        entity.Property(x => x.TimeSent)
            .IsRequired();

        entity.HasOne(x => x.Sender)
            .WithMany(x => x.SentMessages)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => x.TimeSent);
    }
}