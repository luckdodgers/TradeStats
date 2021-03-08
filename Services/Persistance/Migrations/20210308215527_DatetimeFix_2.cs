using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class DatetimeFix_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Datetime",
                table: "Trades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Datetime",
                table: "ClosedTrades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Datetime",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Datetime",
                table: "ClosedTrades");
        }
    }
}
