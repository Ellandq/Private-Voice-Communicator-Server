using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Media;

public class MediaConfiguration : IEntityTypeConfiguration<Domain.Entities.Media.Media>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Media.Media> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.StorageKey)
            .IsRequired()
            .HasMaxLength(512);

        entity.HasIndex(x => x.StorageKey)
            .IsUnique();

        entity.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(127);

        entity.Property(x => x.Size)
            .IsRequired();

        entity.HasOne(x => x.Owner)
            .WithMany(x => x.OwnedMedia)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}