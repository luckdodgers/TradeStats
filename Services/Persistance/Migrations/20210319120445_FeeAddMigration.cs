using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class FeeAddMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sum",
                table: "Trades",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sum",
                table: "Trades");
        }
    }
}
