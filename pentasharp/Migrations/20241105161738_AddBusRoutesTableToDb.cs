using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pentasharp.Migrations
{
    /// <inheritdoc />
    public partial class AddBusRoutesTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromLocation = table.Column<string>(maxLength: 100, nullable: false),
                    ToLocation = table.Column<string>(maxLength: 100, nullable: false),
                    EstimatedDuration = table.Column<TimeSpan>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.RouteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BusRoutes");
        }
    }
}
