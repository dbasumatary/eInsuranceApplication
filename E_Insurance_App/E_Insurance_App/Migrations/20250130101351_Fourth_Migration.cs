using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Insurance_App.Migrations
{
    /// <inheritdoc />
    public partial class Fourth_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsurancePlans",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlanDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlans", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Schemes",
                columns: table => new
                {
                    SchemeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchemeDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchemeFactor = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemes", x => x.SchemeID);
                    table.ForeignKey(
                        name: "FK_Schemes_InsurancePlans_PlanID",
                        column: x => x.PlanID,
                        principalTable: "InsurancePlans",
                        principalColumn: "PlanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_PlanID",
                table: "Schemes",
                column: "PlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schemes");

            migrationBuilder.DropTable(
                name: "InsurancePlans");
        }
    }
}
