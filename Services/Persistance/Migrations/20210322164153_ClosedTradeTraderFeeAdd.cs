using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class ClosedTradeTraderFeeAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoundFee",
                table: "ClosedTrades",
                newName: "TraderFee");

            migrationBuilder.AddColumn<string>(
                name: "ExchangeRoundFee",
                table: "ClosedTrades",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRoundFee",
                table: "ClosedTrades");

            migrationBuilder.RenameColumn(
                name: "TraderFee",
                table: "ClosedTrades",
                newName: "RoundFee");
        }
    }
}
