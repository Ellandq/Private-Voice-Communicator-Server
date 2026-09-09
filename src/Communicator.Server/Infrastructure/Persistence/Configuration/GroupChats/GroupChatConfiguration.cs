using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.GroupChats;

public class GroupChatConfiguration : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(x => x.Description)
            .HasMaxLength(1000);
    }
}