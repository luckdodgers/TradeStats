using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Accounts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
            //        AccountName = table.Column<string>(type: "TEXT", nullable: true),
            //        Exchange = table.Column<short>(type: "INTEGER", nullable: false),
            //        Fee = table.Column<string>(type: "TEXT", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Accounts", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClosedTrades",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        AccountId = table.Column<int>(type: "INTEGER", nullable: false),
            //        Datetime = table.Column<long>(type: "INTEGER", nullable: false),
            //        OpenPrice = table.Column<string>(type: "TEXT", nullable: false),
            //        ClosePrice = table.Column<string>(type: "TEXT", nullable: false),
            //        Amount = table.Column<string>(type: "TEXT", nullable: false),
            //        RoundFee = table.Column<string>(type: "TEXT", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClosedTrades", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ClosedTrades_Accounts_AccountId",
            //            column: x => x.AccountId,
            //            principalTable: "Accounts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Trades",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        AccountId = table.Column<int>(type: "INTEGER", nullable: false),
            //        Datetime = table.Column<long>(type: "INTEGER", nullable: false),
            //        Price = table.Column<string>(type: "TEXT", nullable: false),
            //        Amount = table.Column<string>(type: "TEXT", nullable: false),
            //        Fee = table.Column<string>(type: "TEXT", nullable: false),
            //        Residue = table.Column<string>(type: "TEXT", nullable: false),
            //        IsClosed = table.Column<bool>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Trades", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Trades_Accounts_AccountId",
            //            column: x => x.AccountId,
            //            principalTable: "Accounts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClosedTrades_AccountId",
            //    table: "ClosedTrades",
            //    column: "AccountId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Trades_AccountId",
            //    table: "Trades",
            //    column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedTrades");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
