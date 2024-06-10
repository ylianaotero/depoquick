using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntry_Users_UserId",
                table: "LogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Deposits_DepositId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rating_RatingId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameTable(
                name: "LogEntry",
                newName: "LogEntries");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_DepositId",
                table: "Ratings",
                newName: "IX_Ratings_DepositId");

            migrationBuilder.RenameIndex(
                name: "IX_LogEntry_UserId",
                table: "LogEntries",
                newName: "IX_LogEntries_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntries",
                table: "LogEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_Users_UserId",
                table: "LogEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Deposits_DepositId",
                table: "Ratings",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Ratings_RatingId",
                table: "Reservations",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_Users_UserId",
                table: "LogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Deposits_DepositId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Ratings_RatingId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntries",
                table: "LogEntries");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameTable(
                name: "LogEntries",
                newName: "LogEntry");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_DepositId",
                table: "Rating",
                newName: "IX_Rating_DepositId");

            migrationBuilder.RenameIndex(
                name: "IX_LogEntries_UserId",
                table: "LogEntry",
                newName: "IX_LogEntry_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntry_Users_UserId",
                table: "LogEntry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Deposits_DepositId",
                table: "Rating",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rating_RatingId",
                table: "Reservations",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
