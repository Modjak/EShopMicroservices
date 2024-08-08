using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customeres_CustomerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customeres",
                table: "Customeres");

            migrationBuilder.RenameTable(
                name: "Customeres",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Customeres_Email",
                table: "Customers",
                newName: "IX_Customers_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customeres");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Email",
                table: "Customeres",
                newName: "IX_Customeres_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customeres",
                table: "Customeres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customeres_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customeres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
