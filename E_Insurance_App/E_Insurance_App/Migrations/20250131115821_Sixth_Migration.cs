using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Insurance_App.Migrations
{
    /// <inheritdoc />
    public partial class Sixth_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Premiums",
                columns: table => new
                {
                    PremiumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    PolicyID = table.Column<int>(type: "int", nullable: false),
                    SchemeID = table.Column<int>(type: "int", nullable: false),
                    BaseRate = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CalculatedPremium = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premiums", x => x.PremiumID);
                    table.ForeignKey(
                        name: "FK_Premiums_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Premiums_Policies_PolicyID",
                        column: x => x.PolicyID,
                        principalTable: "Policies",
                        principalColumn: "PolicyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Premiums_Schemes_SchemeID",
                        column: x => x.SchemeID,
                        principalTable: "Schemes",
                        principalColumn: "SchemeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Premiums_CustomerID",
                table: "Premiums",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Premiums_PolicyID",
                table: "Premiums",
                column: "PolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_Premiums_SchemeID",
                table: "Premiums",
                column: "SchemeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Premiums");
        }
    }
}
