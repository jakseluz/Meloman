using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meloman4.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistAndCategoryToTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistMark_User_UserId",
                table: "ArtistMark");

            migrationBuilder.DropForeignKey(
                name: "FK_Track_Artist_AuthorId",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Track_AuthorId",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_ArtistMark_UserId",
                table: "ArtistMark");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Track",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Track");

            migrationBuilder.CreateIndex(
                name: "IX_Track_AuthorId",
                table: "Track",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMark_UserId",
                table: "ArtistMark",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistMark_User_UserId",
                table: "ArtistMark",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Artist_AuthorId",
                table: "Track",
                column: "AuthorId",
                principalTable: "Artist",
                principalColumn: "Id");
        }
    }
}
