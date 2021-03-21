using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class RemoveResidueFromOpenTradeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Residue",
                table: "OpenTrades");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Residue",
                table: "OpenTrades",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
