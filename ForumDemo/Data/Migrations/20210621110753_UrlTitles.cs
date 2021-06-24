using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDemo.Data.Migrations
{
    public partial class UrlTitles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Forums",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Forums");
        }
    }
}
