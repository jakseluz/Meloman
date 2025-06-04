using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meloman4.Migrations
{
    /// <inheritdoc />
    public partial class SwitchedCategoryMarkToPlainId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMark_Category_CategoryId",
                table: "CategoryMark");

            migrationBuilder.DropIndex(
                name: "IX_CategoryMark_CategoryId",
                table: "CategoryMark");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CategoryMark_CategoryId",
                table: "CategoryMark",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMark_Category_CategoryId",
                table: "CategoryMark",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
