using Communicator.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Communicator.Server.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
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

        ConfigureUsers(modelBuilder);
        ConfigureGroupChats(modelBuilder);
        ConfigureVoiceCalls(modelBuilder);
        ConfigureMessages(modelBuilder);
        ConfigureMedia(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
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
        });
    }

    private static void ConfigureGroupChats(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupChat>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .HasMaxLength(1000);
        });

        // GroupChat <-> User
        modelBuilder.Entity<GroupChatMember>(entity =>
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
        });
    }

    private static void ConfigureVoiceCalls(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GcVoiceCall>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.GroupChat)
                .WithMany(x => x.VoiceCalls)
                .HasForeignKey(x => x.GroupChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GcVoiceCallParticipant>(entity =>
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
        });
    }

    private static void ConfigureMessages(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
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
        });

        // Message -> GroupChat
        modelBuilder.Entity<GcMessage>(entity =>
        {
            entity.HasKey(x => x.MessageId);

            entity.HasOne(x => x.Message)
                .WithOne(x => x.GroupChatMessage)
                .HasForeignKey<GcMessage>(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.GroupChat)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.GroupChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.GroupChatId,
                x.MessageId
            });
        });

        // Message -> DM
        modelBuilder.Entity<DmMessage>(entity =>
        {
            entity.HasKey(x => x.MessageId);

            entity.HasOne(x => x.Message)
                .WithOne(x => x.DirectMessage)
                .HasForeignKey<DmMessage>(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedDirectMessages)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Reactions
        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ReactionEm)
                .IsRequired()
                .HasMaxLength(16);

            entity.HasOne(x => x.Message)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.MessageId,
                x.UserId,
                x.ReactionEm
            }).IsUnique();

            entity.HasIndex(x => new
            {
                x.MessageId,
                x.TimeAdded
            });
        });
    }

    private static void ConfigureMedia(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>(entity =>
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
        });

        // Message <-> Media
        modelBuilder.Entity<MessageAttachment>(entity =>
        {
            entity.HasKey(x => new
            {
                x.MessageId,
                x.MediaId
            });

            entity.HasOne(x => x.Message)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Media)
                .WithMany(x => x.MessageAttachments)
                .HasForeignKey(x => x.MediaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // User -> ProfilePicture
        modelBuilder.Entity<ProfilePicture>(entity =>
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
        });

        // GroupChat -> Icon
        modelBuilder.Entity<GcIcon>(entity =>
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
        });
    }
}