using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Insurance_App.Migrations
{
    /// <inheritdoc />
    public partial class Ninth_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Commissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentProcessedDate",
                table: "Commissions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "PaymentProcessedDate",
                table: "Commissions");
        }
    }
}
