using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaProject.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToApply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_status_orders_OrderId",
                table: "status");

            migrationBuilder.DropIndex(
                name: "IX_status_OrderId",
                table: "status");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "resume",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "resume");

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
    }
}
