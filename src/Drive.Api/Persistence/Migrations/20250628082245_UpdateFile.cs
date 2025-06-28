using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "size_in_bytes",
                table: "files",
                newName: "size");

            migrationBuilder.RenameColumn(
                name: "original_file_name",
                table: "files",
                newName: "name");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "files",
                type: "char(26)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "size",
                table: "files",
                newName: "size_in_bytes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "files",
                newName: "original_file_name");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "files",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(26)");
        }
    }
}
