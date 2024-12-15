using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules",
                column: "BusCompanyId",
                principalTable: "BusCompanies",
                principalColumn: "BusCompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules",
                column: "BusCompanyId",
                principalTable: "BusCompanies",
                principalColumn: "BusCompanyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
