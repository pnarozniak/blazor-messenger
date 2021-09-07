using Microsoft.EntityFrameworkCore.Migrations;

namespace messanger.Server.Data.Migrations
{
    public partial class added_IsPrivate_column_to_Conversation_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Conversation");
        }
    }
}
