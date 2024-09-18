using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaProject.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "status");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "orders");
        }
    }
}
