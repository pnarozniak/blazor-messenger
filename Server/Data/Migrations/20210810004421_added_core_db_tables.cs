using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace messanger.Server.Data.Migrations
{
    public partial class added_core_db_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdAvatar",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    IdUser1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUser2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Friendship_pk", x => new { x.IdUser1, x.IdUser2 });
                    table.ForeignKey(
                        name: "Friendship_User1",
                        column: x => x.IdUser1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Friendship_User2",
                        column: x => x.IdUser2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendshipRequest",
                columns: table => new
                {
                    IdSender = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdReceiver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FriendshipRequest_pk", x => new { x.IdSender, x.IdReceiver });
                    table.ForeignKey(
                        name: "FriendshipRequest_Receiver",
                        column: x => x.IdReceiver,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FriendshipRequest_Sender",
                        column: x => x.IdSender,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMember",
                columns: table => new
                {
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdConversation = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConversationMember_pk", x => new { x.IdUser, x.IdConversation });
                    table.ForeignKey(
                        name: "User_ConversationMembers",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    IdMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdConversation = table.Column<int>(type: "int", nullable: false),
                    IdParentMessage = table.Column<int>(type: "int", nullable: true),
                    IdSender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Message_pk", x => x.IdMessage);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Message_ParentMessage",
                        column: x => x.IdParentMessage,
                        principalTable: "Message",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    IdFile = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMessage = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("File_pk", x => x.IdFile);
                    table.ForeignKey(
                        name: "Message_AttachedFiles",
                        column: x => x.IdMessage,
                        principalTable: "Message",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    IdConversation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAvatar = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Conversation_pk", x => x.IdConversation);
                    table.ForeignKey(
                        name: "Conversation_AvatarFile",
                        column: x => x.IdAvatar,
                        principalTable: "File",
                        principalColumn: "IdFile",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers",
                column: "IdAvatar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_IdAvatar",
                table: "Conversation",
                column: "IdAvatar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMember_IdConversation",
                table: "ConversationMember",
                column: "IdConversation");

            migrationBuilder.CreateIndex(
                name: "IX_File_IdMessage",
                table: "File",
                column: "IdMessage");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_IdUser2",
                table: "Friendship",
                column: "IdUser2");

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipRequest_IdReceiver",
                table: "FriendshipRequest",
                column: "IdReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IdConversation",
                table: "Message",
                column: "IdConversation");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IdParentMessage",
                table: "Message",
                column: "IdParentMessage");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers",
                column: "IdAvatar",
                principalTable: "File",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Conversation_ConversationMembers",
                table: "ConversationMember",
                column: "IdConversation",
                principalTable: "Conversation",
                principalColumn: "IdConversation",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Conversation_Message",
                table: "Message",
                column: "IdConversation",
                principalTable: "Conversation",
                principalColumn: "IdConversation",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "Conversation_AvatarFile",
                table: "Conversation");

            migrationBuilder.DropTable(
                name: "ConversationMember");

            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropTable(
                name: "FriendshipRequest");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdAvatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
