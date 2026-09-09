using Communicator.Server.Domain.Entities.GroupChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.GroupChats;

public class GcVoiceCallParticipantConfiguration : IEntityTypeConfiguration<GcVoiceCallParticipant>
{
    public void Configure(EntityTypeBuilder<GcVoiceCallParticipant> entity)
    {
        entity.HasKey(x => new
        {
            x.GcVoiceCallId,
            x.UserId
        });

        entity.HasOne(x => x.VoiceCall)
            .WithMany(x => x.Participants)
            .HasForeignKey(x => x.GcVoiceCallId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.User)
            .WithMany(x => x.VoiceCalls)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}