using Microsoft.EntityFrameworkCore.Migrations;

namespace messanger.Server.Data.Migrations
{
    public partial class added_after_insert_trigger_to_FriendshipRequest_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE TRIGGER CheckOppositteFriendshipRequest ON FriendshipRequest
                  AFTER INSERT
                  AS 
                  BEGIN 
                    DECLARE @exists AS VARCHAR(36)
                    SELECT @exists = '1' FROM FriendshipRequest 
                        WHERE IdSender = (SELECT IdReceiver FROM INSERTED) AND IdReceiver = (SELECT IdSender FROM INSERTED)
                    IF @exists = '1'
                    BEGIN
                        ROLLBACK
                    END
                  END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER CheckFriendship ON FriendshipRequest
                  AFTER INSERT
                  AS 
                  BEGIN 
                    DECLARE @exists AS VARCHAR(36)
                    SELECT @exists = '1' FROM Friendship 
                        WHERE (IdUser1 = (SELECT IdSender FROM INSERTED) AND IdUser2 = (SELECT IdReceiver FROM INSERTED)) 
                            OR (IdUser2 = (SELECT IdSender FROM INSERTED) AND IdUser1 = (SELECT IdReceiver FROM INSERTED))
                    IF @exists = '1'
                    BEGIN
                        ROLLBACK
                    END
                  END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TRIGGER CheckOppositteFriendshipRequest");

            migrationBuilder.Sql(
                @"DROP TRIGGER CheckFriendship");
        }
    }
}
