using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Migrations
{
    public partial class DepositPromotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositPromotion");

            migrationBuilder.AddColumn<int>(
                name: "DepositId",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_DepositId",
                table: "Promotions",
                column: "DepositId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Deposits_DepositId",
                table: "Promotions",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Deposits_DepositId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_DepositId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "DepositId",
                table: "Promotions");

            migrationBuilder.CreateTable(
                name: "DepositPromotion",
                columns: table => new
                {
                    DepositsId = table.Column<int>(type: "int", nullable: false),
                    PromotionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositPromotion", x => new { x.DepositsId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_DepositPromotion_Deposits_DepositsId",
                        column: x => x.DepositsId,
                        principalTable: "Deposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositPromotion_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositPromotion_PromotionsId",
                table: "DepositPromotion",
                column: "PromotionsId");
        }
    }
}
