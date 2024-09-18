using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaProject.Migrations
{
    /// <inheritdoc />
    public partial class lol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "status");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_status_OrderId",
                table: "status",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_status_orders_OrderId",
                table: "status",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_status_orders_OrderId",
                table: "status");

            migrationBuilder.DropIndex(
                name: "IX_status_OrderId",
                table: "status");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "status");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
