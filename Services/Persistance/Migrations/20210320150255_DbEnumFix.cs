using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class DbEnumFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstCurrency",
                table: "OpenTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondCurrency",
                table: "OpenTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "OpenTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuyCurrency",
                table: "ClosedTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SellCurrency",
                table: "ClosedTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstCurrency",
                table: "OpenTrades");

            migrationBuilder.DropColumn(
                name: "SecondCurrency",
                table: "OpenTrades");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "OpenTrades");

            migrationBuilder.DropColumn(
                name: "BuyCurrency",
                table: "ClosedTrades");

            migrationBuilder.DropColumn(
                name: "SellCurrency",
                table: "ClosedTrades");
        }
    }
}
