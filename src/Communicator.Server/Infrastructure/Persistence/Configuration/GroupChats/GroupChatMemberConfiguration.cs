using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.GroupChats;

public class GroupChatMemberConfiguration : IEntityTypeConfiguration<GroupChatMember>
{
    public void Configure(EntityTypeBuilder<GroupChatMember> entity)
    {
        entity.HasKey(x => new
        {
            x.GroupChatId,
            x.UserId
        });

        entity.HasOne(x => x.GroupChat)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.User)
            .WithMany(x => x.GroupChats)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}