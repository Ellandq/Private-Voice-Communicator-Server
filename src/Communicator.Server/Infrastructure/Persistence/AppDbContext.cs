using Communicator.Server.Domain.Entities;
using Communicator.Server.Domain.Entities.GroupChats;
using Communicator.Server.Domain.Entities.Media;
using Communicator.Server.Domain.Entities.Messages;
using Communicator.Server.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Communicator.Server.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<GroupChat> GroupChats => Set<GroupChat>();
    public DbSet<GroupChatMember> GroupChatMembers => Set<GroupChatMember>();

    public DbSet<GcVoiceCall> GcVoiceCalls => Set<GcVoiceCall>();
    public DbSet<GcVoiceCallParticipant> GcVoiceCallParticipants => Set<GcVoiceCallParticipant>();

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<GcMessage> GcMessages => Set<GcMessage>();
    public DbSet<DmMessage> DmMessages => Set<DmMessage>();

    public DbSet<Reaction> Reactions => Set<Reaction>();

    public DbSet<Media> Media => Set<Media>();
    public DbSet<MessageAttachment> MessageAttachments => Set<MessageAttachment>();

    public DbSet<ProfilePicture> ProfilePictures => Set<ProfilePicture>();
    public DbSet<GcIcon> GcIcons => Set<GcIcon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}