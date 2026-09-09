using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.GroupChats;

public class GcIconConfiguration : IEntityTypeConfiguration<GcIcon>
{
    public void Configure(EntityTypeBuilder<GcIcon> entity)
    {
        entity.HasKey(x => x.GroupChatId);

        entity.HasOne(x => x.GroupChat)
            .WithOne(x => x.Icon)
            .HasForeignKey<GcIcon>(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Media)
            .WithOne(x => x.GroupChatIcon)
            .HasForeignKey<GcIcon>(x => x.MediaId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => x.MediaId)
            .IsUnique();
    }
}