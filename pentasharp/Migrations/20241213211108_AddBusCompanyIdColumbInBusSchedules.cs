using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class AddBusCompanyIdColumbInBusSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusCompanyId",
                table: "BusSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_BusCompanyId",
                table: "BusSchedules",
                column: "BusCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules",
                column: "BusCompanyId",
                principalTable: "BusCompanies",
                principalColumn: "BusCompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusSchedules_BusCompanies_BusCompanyId",
                table: "BusSchedules");

            migrationBuilder.DropIndex(
                name: "IX_BusSchedules_BusCompanyId",
                table: "BusSchedules");

            migrationBuilder.DropColumn(
                name: "BusCompanyId",
                table: "BusSchedules");
        }
    }
}
