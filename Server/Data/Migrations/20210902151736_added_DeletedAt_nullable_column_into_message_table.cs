using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace messanger.Server.Data.Migrations
{
    public partial class added_DeletedAt_nullable_column_into_message_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Message",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Message");
        }
    }
}
