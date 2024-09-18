using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaProject.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Syringe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiquidName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirPressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirVolume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FillSpeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FillRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syringe", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Syringe");
        }
    }
}
