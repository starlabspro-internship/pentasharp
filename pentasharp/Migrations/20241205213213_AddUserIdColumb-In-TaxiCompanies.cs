using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumbInTaxiCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TaxiCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxiCompanies_UserId",
                table: "TaxiCompanies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxiCompanies_Users_UserId",
                table: "TaxiCompanies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxiCompanies_Users_UserId",
                table: "TaxiCompanies");

            migrationBuilder.DropIndex(
                name: "IX_TaxiCompanies_UserId",
                table: "TaxiCompanies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaxiCompanies");
        }
    }
}
