using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class EntitiesConfigurated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Trades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "ClosedTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trades_AccountId",
                table: "Trades",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedTrades_AccountId",
                table: "ClosedTrades",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedTrades_Accounts_AccountId",
                table: "ClosedTrades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosedTrades_Accounts_AccountId",
                table: "ClosedTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_AccountId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_ClosedTrades_AccountId",
                table: "ClosedTrades");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "ClosedTrades");
        }
    }
}
