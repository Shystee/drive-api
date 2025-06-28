using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "album_id",
                table: "files",
                type: "char(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_files_album_id",
                table: "files",
                column: "album_id");

            migrationBuilder.AddForeignKey(
                name: "fk_files_album_id",
                table: "files",
                column: "album_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_album_id",
                table: "files");

            migrationBuilder.DropIndex(
                name: "ix_files_album_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "album_id",
                table: "files");
        }
    }
}
