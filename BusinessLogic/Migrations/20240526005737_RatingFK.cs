using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Migrations
{
    public partial class RatingFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Ratings_RatingId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RatingId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ReservationId",
                table: "Ratings",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Reservations_ReservationId",
                table: "Ratings",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Reservations_ReservationId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ReservationId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RatingId",
                table: "Reservations",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Ratings_RatingId",
                table: "Reservations",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
