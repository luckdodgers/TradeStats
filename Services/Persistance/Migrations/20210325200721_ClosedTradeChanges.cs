using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class ClosedTradeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellCurrency",
                table: "ClosedTrades",
                newName: "SecondCurrency");

            migrationBuilder.RenameColumn(
                name: "BuyCurrency",
                table: "ClosedTrades",
                newName: "Position");

            migrationBuilder.AddColumn<short>(
                name: "FeeCurrency",
                table: "OpenTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "FirstCurrency",
                table: "ClosedTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeCurrency",
                table: "OpenTrades");

            migrationBuilder.DropColumn(
                name: "FirstCurrency",
                table: "ClosedTrades");

            migrationBuilder.RenameColumn(
                name: "SecondCurrency",
                table: "ClosedTrades",
                newName: "SellCurrency");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "ClosedTrades",
                newName: "BuyCurrency");
        }
    }
}
