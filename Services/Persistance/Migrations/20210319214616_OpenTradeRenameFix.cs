using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class OpenTradeRenameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trades",
                table: "Trades");

            migrationBuilder.RenameTable(
                name: "Trades",
                newName: "OpenTrades");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_AccountId",
                table: "OpenTrades",
                newName: "IX_OpenTrades_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenTrades",
                table: "OpenTrades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenTrades_Accounts_AccountId",
                table: "OpenTrades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenTrades_Accounts_AccountId",
                table: "OpenTrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenTrades",
                table: "OpenTrades");

            migrationBuilder.RenameTable(
                name: "OpenTrades",
                newName: "Trades");

            migrationBuilder.RenameIndex(
                name: "IX_OpenTrades_AccountId",
                table: "Trades",
                newName: "IX_Trades_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trades",
                table: "Trades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
