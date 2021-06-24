using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDemo.Data.Migrations
{
    public partial class removeoriginidproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginPostId",
                table: "Topics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginPostId",
                table: "Topics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
