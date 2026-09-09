using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.GroupChats;

public class GcVoiceCallConfiguration : IEntityTypeConfiguration<GcVoiceCall>
{
    public void Configure(EntityTypeBuilder<GcVoiceCall> entity)
    {
        entity.HasKey(x => x.Id);

        entity.HasOne(x => x.GroupChat)
            .WithMany(x => x.VoiceCalls)
            .HasForeignKey(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}