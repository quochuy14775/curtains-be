using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace curtains_be.Migrations
{
    /// <inheritdoc />
    public partial class _106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ImageRight");

            migrationBuilder.AddColumn<string>(
                name: "ImageDetail",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFront",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageLeft",
                table: "Products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDetail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageFront",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageLeft",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ImageRight",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
