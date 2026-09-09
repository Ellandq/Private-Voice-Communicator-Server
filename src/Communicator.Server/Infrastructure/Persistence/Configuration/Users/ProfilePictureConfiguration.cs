using Communicator.Server.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Users;

public class ProfilePictureConfiguration : IEntityTypeConfiguration<ProfilePicture>
{
    public void Configure(EntityTypeBuilder<ProfilePicture> entity)
    {
        entity.HasKey(x => x.UserId);

        entity.HasOne(x => x.User)
            .WithOne(x => x.ProfilePicture)
            .HasForeignKey<ProfilePicture>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Media)
            .WithOne(x => x.ProfilePicture)
            .HasForeignKey<ProfilePicture>(x => x.MediaId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => x.MediaId)
            .IsUnique();
    }
}