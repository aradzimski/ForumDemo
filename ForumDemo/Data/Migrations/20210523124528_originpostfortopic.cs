using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDemo.Data.Migrations
{
    public partial class originpostfortopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginPostId",
                table: "Topics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginPostId",
                table: "Topics");
        }
    }
}
