using Microsoft.EntityFrameworkCore.Migrations;

namespace messanger.Server.Data.Migrations
{
    public partial class modified_file_relations_to_non_mandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "Conversation_AvatarFile",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_IdAvatar",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdAvatar",
                table: "Conversation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdAvatar",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_IdAvatar",
                table: "Conversation",
                column: "IdAvatar",
                unique: true,
                filter: "[IdAvatar] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers",
                column: "IdAvatar",
                unique: true,
                filter: "[IdAvatar] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers",
                column: "IdAvatar",
                principalTable: "File",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Conversation_AvatarFile",
                table: "Conversation",
                column: "IdAvatar",
                principalTable: "File",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "Conversation_AvatarFile",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_IdAvatar",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdAvatar",
                table: "Conversation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdAvatar",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_IdAvatar",
                table: "Conversation",
                column: "IdAvatar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdAvatar",
                table: "AspNetUsers",
                column: "IdAvatar",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "User_AvatarFile",
                table: "AspNetUsers",
                column: "IdAvatar",
                principalTable: "File",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Conversation_AvatarFile",
                table: "Conversation",
                column: "IdAvatar",
                principalTable: "File",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
