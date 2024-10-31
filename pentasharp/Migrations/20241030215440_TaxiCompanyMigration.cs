using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class TaxiCompanyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxiCompanies",
                columns: table => new
                {
                    TaxiCompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiCompanies", x => x.TaxiCompanyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxiCompanies_CompanyName",
                table: "TaxiCompanies",
                column: "CompanyName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxiCompanies");
        }
    }
}
