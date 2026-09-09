using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Communicator.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusMessage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GcVoiceCalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupChatId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcVoiceCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GcVoiceCalls_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMembers",
                columns: table => new
                {
                    GroupChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMembers", x => new { x.GroupChatId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupChatMembers_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupChatMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StorageKey = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 127, nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CipherText = table.Column<string>(type: "TEXT", nullable: false),
                    TimeSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GcVoiceCallParticipants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GcVoiceCallId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcVoiceCallParticipants", x => new { x.GcVoiceCallId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GcVoiceCallParticipants_GcVoiceCalls_GcVoiceCallId",
                        column: x => x.GcVoiceCallId,
                        principalTable: "GcVoiceCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GcVoiceCallParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GcIcons",
                columns: table => new
                {
                    GroupChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MediaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcIcons", x => x.GroupChatId);
                    table.ForeignKey(
                        name: "FK_GcIcons_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GcIcons_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePictures",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MediaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePictures", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_ProfilePictures_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfilePictures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DmMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_DmMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DmMessages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GcMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupChatId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GcMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_GcMessages_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GcMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageAttachments",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MediaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageAttachments", x => new { x.MessageId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_MessageAttachments_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageAttachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReactionEm = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DmMessages_ReceiverId",
                table: "DmMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_GcIcons_MediaId",
                table: "GcIcons",
                column: "MediaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GcMessages_GroupChatId_MessageId",
                table: "GcMessages",
                columns: new[] { "GroupChatId", "MessageId" });

            migrationBuilder.CreateIndex(
                name: "IX_GcVoiceCallParticipants_UserId",
                table: "GcVoiceCallParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GcVoiceCalls_GroupChatId",
                table: "GcVoiceCalls",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_UserId",
                table: "GroupChatMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_OwnerId",
                table: "Media",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_StorageKey",
                table: "Media",
                column: "StorageKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachments_MediaId",
                table: "MessageAttachments",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TimeSent",
                table: "Messages",
                column: "TimeSent");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_MediaId",
                table: "ProfilePictures",
                column: "MediaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId_TimeAdded",
                table: "Reactions",
                columns: new[] { "MessageId", "TimeAdded" });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId_UserId_ReactionEm",
                table: "Reactions",
                columns: new[] { "MessageId", "UserId", "ReactionEm" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmMessages");

            migrationBuilder.DropTable(
                name: "GcIcons");

            migrationBuilder.DropTable(
                name: "GcMessages");

            migrationBuilder.DropTable(
                name: "GcVoiceCallParticipants");

            migrationBuilder.DropTable(
                name: "GroupChatMembers");

            migrationBuilder.DropTable(
                name: "MessageAttachments");

            migrationBuilder.DropTable(
                name: "ProfilePictures");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "GcVoiceCalls");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "GroupChats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
