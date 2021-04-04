using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class ClosedTradeColumnsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenPrice",
                table: "ClosedTrades",
                newName: "SellPrice");

            migrationBuilder.RenameColumn(
                name: "ClosePrice",
                table: "ClosedTrades",
                newName: "BuyPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellPrice",
                table: "ClosedTrades",
                newName: "OpenPrice");

            migrationBuilder.RenameColumn(
                name: "BuyPrice",
                table: "ClosedTrades",
                newName: "ClosePrice");
        }
    }
}
