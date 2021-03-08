using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeStats.Migrations
{
    public partial class UnsupportedTypesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Residue",
                table: "Trades",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Fee",
                table: "Trades",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<byte[]>(
                name: "Amount",
                table: "Trades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Price",
                table: "Trades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Amount",
                table: "ClosedTrades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClosePrice",
                table: "ClosedTrades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "OpenPrice",
                table: "ClosedTrades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RoundFee",
                table: "ClosedTrades",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Fee",
                table: "Accounts",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ClosedTrades");

            migrationBuilder.DropColumn(
                name: "ClosePrice",
                table: "ClosedTrades");

            migrationBuilder.DropColumn(
                name: "OpenPrice",
                table: "ClosedTrades");

            migrationBuilder.DropColumn(
                name: "RoundFee",
                table: "ClosedTrades");

            migrationBuilder.AlterColumn<decimal>(
                name: "Residue",
                table: "Trades",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Trades",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }
    }
}
