using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumbInTableBusCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BusCompanies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BusCompanies_UserId",
                table: "BusCompanies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusCompanies_Users_UserId",
                table: "BusCompanies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusCompanies_Users_UserId",
                table: "BusCompanies");

            migrationBuilder.DropIndex(
                name: "IX_BusCompanies_UserId",
                table: "BusCompanies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BusCompanies");
        }
    }
}
