using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meloman4.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToArtistMark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistMark_Artist_ArtistId",
                table: "ArtistMark");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_UserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMark_User_UserId",
                table: "CategoryMark");

            migrationBuilder.DropIndex(
                name: "IX_CategoryMark_UserId",
                table: "CategoryMark");

            migrationBuilder.DropIndex(
                name: "IX_Category_UserId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_ArtistMark_ArtistId",
                table: "ArtistMark");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CategoryMark_UserId",
                table: "CategoryMark",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UserId",
                table: "Category",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMark_ArtistId",
                table: "ArtistMark",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistMark_Artist_ArtistId",
                table: "ArtistMark",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_UserId",
                table: "Category",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMark_User_UserId",
                table: "CategoryMark",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
