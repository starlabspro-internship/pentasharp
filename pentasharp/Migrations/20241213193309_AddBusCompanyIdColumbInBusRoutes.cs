using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class AddBusCompanyIdColumbInBusRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusCompanyId",
                table: "BusRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BusRoutes_BusCompanyId",
                table: "BusRoutes",
                column: "BusCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusRoutes_BusCompanies_BusCompanyId",
                table: "BusRoutes",
                column: "BusCompanyId",
                principalTable: "BusCompanies",
                principalColumn: "BusCompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusRoutes_BusCompanies_BusCompanyId",
                table: "BusRoutes");

            migrationBuilder.DropIndex(
                name: "IX_BusRoutes_BusCompanyId",
                table: "BusRoutes");

            migrationBuilder.DropColumn(
                name: "BusCompanyId",
                table: "BusRoutes");
        }
    }
}
