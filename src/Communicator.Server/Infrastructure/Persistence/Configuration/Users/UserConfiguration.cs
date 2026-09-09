using Communicator.Server.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communicator.Server.Infrastructure.Persistence.Configuration.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(32);

        entity.HasIndex(x => x.Username)
            .IsUnique();

        entity.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(320);

        entity.HasIndex(x => x.Email)
            .IsUnique();

        entity.Property(x => x.PasswordHash)
            .IsRequired();

        entity.Property(x => x.Status)
            .IsRequired();

        entity.Property(x => x.StatusMessage)
            .HasMaxLength(256);
    }
}