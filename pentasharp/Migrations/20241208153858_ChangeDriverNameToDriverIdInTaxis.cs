using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDriverNameToDriverIdInTaxis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Taxis_DriverName",
                table: "Taxis");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Taxis");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Taxis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverUserId",
                table: "Taxis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxis_DriverId",
                table: "Taxis",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Taxis_DriverUserId",
                table: "Taxis",
                column: "DriverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taxis_Users_DriverId",
                table: "Taxis",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxis_Users_DriverUserId",
                table: "Taxis",
                column: "DriverUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taxis_Users_DriverId",
                table: "Taxis");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxis_Users_DriverUserId",
                table: "Taxis");

            migrationBuilder.DropIndex(
                name: "IX_Taxis_DriverId",
                table: "Taxis");

            migrationBuilder.DropIndex(
                name: "IX_Taxis_DriverUserId",
                table: "Taxis");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Taxis");

            migrationBuilder.DropColumn(
                name: "DriverUserId",
                table: "Taxis");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Taxis",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Taxis_DriverName",
                table: "Taxis",
                column: "DriverName",
                unique: true);
        }
    }
}
